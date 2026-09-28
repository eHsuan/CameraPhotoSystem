using System;
using System.IO;
using log4net;

namespace CameraPhotoSystem.Utils
{
    public static class Logger
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Logger));

        public static void Info(string message)
        {
            log.Info(message);
        }

        public static void Error(string message, Exception ex = null)
        {
            if (ex != null)
                log.Error(message, ex);
            else
                log.Error(message);
        }

        public static void Debug(string message)
        {
            log.Debug(message);
        }

        public static void CleanupOldLogs(int keepDays = 30)
        {
            try
            {
                string logDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
                if (!Directory.Exists(logDir)) return;

                var cutoffDate = DateTime.Now.AddDays(-keepDays).Date;
                var dirInfo = new DirectoryInfo(logDir);

                foreach (var subDir in dirInfo.GetDirectories())
                {
                    try
                    {
                        bool isExpired = false;
                        // 支援新格式 yyyyMMdd 與舊格式 yyyy-MM-dd
                        if (DateTime.TryParseExact(subDir.Name, new[] { "yyyyMMdd", "yyyy-MM-dd" },
                            System.Globalization.CultureInfo.InvariantCulture,
                            System.Globalization.DateTimeStyles.None, out DateTime dirDate))
                        {
                            if (dirDate < cutoffDate) isExpired = true;
                        }
                        else if (subDir.LastWriteTime < cutoffDate)
                        {
                            isExpired = true;
                        }

                        if (isExpired)
                        {
                            subDir.Delete(true);
                            Info(string.Format("已清理超過 {0} 天的歷史 Log 目錄: {1}", keepDays, subDir.FullName));
                        }
                    }
                    catch (Exception ex)
                    {
                        // 略過單一被鎖定的資料夾
                        Error("清理單一歷史 Log 目錄失敗: " + subDir.FullName, ex);
                    }
                }
            }
            catch (Exception ex)
            {
                Error("清理歷史 Log 目錄時發生例外", ex);
            }
        }
    }
}