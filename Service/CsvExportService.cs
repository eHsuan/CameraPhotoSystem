using System;
using System.IO;
using System.Text;
using System.Threading;
using CameraPhotoSystem.Config;
using CameraPhotoSystem.Utils;

namespace CameraPhotoSystem.Service
{
    public class CsvExportResult
    {
        public bool IsSuccess { get; set; }
        public string LocalFilePath { get; set; }
        public string TargetFilePath { get; set; }
        public string ErrorMessage { get; set; }
    }

    public class CsvExportService
    {
        private static readonly string LocalCsvDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "CSV");

        public CsvExportResult ExportAndUploadSync(string dmc, Action<string> onLog = null)
        {
            var result = new CsvExportResult { IsSuccess = false };

            if (string.IsNullOrWhiteSpace(dmc))
            {
                result.ErrorMessage = "DMC is empty";
                return result;
            }

            try
            {
                // 1. 確保本地 CSV 目錄與當日日期目錄存在
                string dateFolder = DateTime.Now.ToString("yyyyMMdd");
                string localTargetDir = Path.Combine(LocalCsvDirectory, dateFolder);
                if (!Directory.Exists(localTargetDir))
                {
                    Directory.CreateDirectory(localTargetDir);
                }

                // 2. 建立本地 CSV 檔案 (格式比照範例)
                string localFilePath = Path.Combine(localTargetDir, dmc + ".csv");
                result.LocalFilePath = localFilePath;

                string timeStr = DateTime.Now.ToString("yyyy/M/d HH:mm:ss");
                string content = string.Format("DMC,DateTime,Line\r\n{0},{1},{2}\r\n", dmc, timeStr, AppConfig.LineName);

                File.WriteAllText(localFilePath, content, new UTF8Encoding(false));
                Logger.Info(string.Format("本地 CSV 已產生: {0}", localFilePath));

                // 3. 上傳至目標目錄 (含 3 次重試機制)
                string uploadRoot = AppConfig.UploadPath;
                string targetDir = Path.Combine(uploadRoot, dateFolder);
                string targetFilePath = Path.Combine(targetDir, dmc + ".csv");
                result.TargetFilePath = targetFilePath;

                bool uploadSuccess = false;
                string lastError = string.Empty;

                // 嘗試次數: 第一次常規嘗試 + 3 次重試 = 總共 4 次
                for (int attempt = 1; attempt <= 4; attempt++)
                {
                    try
                    {
                        if (!Directory.Exists(targetDir))
                        {
                            Directory.CreateDirectory(targetDir);
                        }

                        File.Copy(localFilePath, targetFilePath, true);
                        uploadSuccess = true;
                        Logger.Info(string.Format("CSV 成功上傳至: {0}", targetFilePath));
                        break;
                    }
                    catch (Exception ex)
                    {
                        lastError = ex.Message;
                        Logger.Error(string.Format("CSV 上傳嘗試第 {0} 次失敗: {1}", attempt, ex.Message), ex);

                        if (attempt < 4)
                        {
                            onLog?.Invoke(string.Format(L.T("LogCsvUploadRetry"), attempt));
                            Thread.Sleep(1000); // 間隔 1 秒後重試
                        }
                    }
                }

                result.IsSuccess = uploadSuccess;
                result.ErrorMessage = lastError;

                // 4. 清理本地端超過 30 天的舊 CSV 檔案
                CleanupOldLocalCsvFiles();
            }
            catch (Exception ex)
            {
                result.ErrorMessage = ex.Message;
                Logger.Error("產生或處理 CSV 流程發生未預期例外", ex);
            }

            return result;
        }

        public CsvExportResult ExportAndUploadTest(string targetUploadRoot, string lineName)
        {
            var result = new CsvExportResult { IsSuccess = false };

            if (string.IsNullOrWhiteSpace(targetUploadRoot))
            {
                result.ErrorMessage = "Upload path is empty";
                return result;
            }

            try
            {
                // 1. 確保本地 CSV 目錄與當日日期目錄存在
                string dateFolder = DateTime.Now.ToString("yyyyMMdd");
                string localTargetDir = Path.Combine(LocalCsvDirectory, dateFolder);
                if (!Directory.Exists(localTargetDir))
                {
                    Directory.CreateDirectory(localTargetDir);
                }

                // 2. 建立測試 CSV 檔案 (以 TEST_ 開頭加上時間戳記)
                string testDmc = "TEST_SAMPLE_" + DateTime.Now.ToString("yyyyMMdd_HHmmss");
                string localFilePath = Path.Combine(localTargetDir, testDmc + ".csv");
                result.LocalFilePath = localFilePath;

                string currentLine = string.IsNullOrWhiteSpace(lineName) ? AppConfig.LineName : lineName;
                string timeStr = DateTime.Now.ToString("yyyy/M/d HH:mm:ss");
                string content = string.Format("DMC,DateTime,Line\r\n{0},{1},{2}\r\n", testDmc, timeStr, currentLine);

                File.WriteAllText(localFilePath, content, new UTF8Encoding(false));
                Logger.Info(string.Format("測試本地 CSV 已產生: {0}", localFilePath));

                // 3. 依照目前路徑規則上傳至目標目錄 (yyyyMMdd 子目錄)
                string targetDir = Path.Combine(targetUploadRoot, dateFolder);
                string targetFilePath = Path.Combine(targetDir, testDmc + ".csv");
                result.TargetFilePath = targetFilePath;

                if (!Directory.Exists(targetDir))
                {
                    Directory.CreateDirectory(targetDir);
                }

                File.Copy(localFilePath, targetFilePath, true);
                result.IsSuccess = true;
                Logger.Info(string.Format("測試 CSV 成功複製至目標路徑: {0}", targetFilePath));

                // 4. 清理本地端超過 30 天的舊 CSV 檔案
                CleanupOldLocalCsvFiles();
            }
            catch (Exception ex)
            {
                result.ErrorMessage = ex.Message;
                Logger.Error("測試 CSV 產生或上傳過程發生例外", ex);
            }

            return result;
        }

        private void CleanupOldLocalCsvFiles()
        {
            try
            {
                if (!Directory.Exists(LocalCsvDirectory)) return;

                var cutoffDate = DateTime.Now.AddDays(-30);
                var dirInfo = new DirectoryInfo(LocalCsvDirectory);

                // 1. 清理本地根目錄下過去遺留的舊 CSV 檔案 (向下相容)
                var rootFiles = dirInfo.GetFiles("*.csv");
                foreach (var file in rootFiles)
                {
                    try
                    {
                        if (file.LastWriteTime < cutoffDate)
                        {
                            file.Delete();
                            Logger.Info(string.Format("已清理超過 30 天的舊 CSV: {0}", file.FullName));
                        }
                    }
                    catch { }
                }

                // 2. 檢查各日期子目錄 (如 yyyyMMdd)
                var subDirs = dirInfo.GetDirectories();
                foreach (var subDir in subDirs)
                {
                    try
                    {
                        bool isExpired = false;
                        if (DateTime.TryParseExact(subDir.Name, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out DateTime dirDate))
                        {
                            if (dirDate < cutoffDate.Date) isExpired = true;
                        }
                        else if (subDir.LastWriteTime < cutoffDate)
                        {
                            isExpired = true;
                        }

                        if (isExpired)
                        {
                            subDir.Delete(true);
                            Logger.Info(string.Format("已清理超過 30 天的舊 CSV 目錄: {0}", subDir.FullName));
                        }
                        else
                        {
                            var subFiles = subDir.GetFiles("*.csv");
                            foreach (var file in subFiles)
                            {
                                if (file.LastWriteTime < cutoffDate)
                                {
                                    file.Delete();
                                    Logger.Info(string.Format("已清理超過 30 天的舊 CSV: {0}", file.FullName));
                                }
                            }

                            if (subDir.GetFileSystemInfos().Length == 0)
                            {
                                subDir.Delete();
                            }
                        }
                    }
                    catch { }
                }
            }
            catch (Exception ex)
            {
                Logger.Error("清理歷史 CSV 發生錯誤", ex);
            }
        }
    }
}
