using System;
using System.Configuration;
using System.IO;
using System.Web.Script.Serialization;
using CameraPhotoSystem.Utils;

namespace CameraPhotoSystem.Config
{
    public class SystemSetting
    {
        public string LineName { get; set; } = "FQC_1";
        public string PhotoRootPath { get; set; } = @"C:\Photo";
        public int MaxPhotoCount { get; set; } = 5;
        public int DesiredWidth { get; set; } = 3840;
        public string UploadPath { get; set; } = @"C:\Upload";
    }

    public static class AppConfig
    {
        private static readonly string ConfigPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "app_settings.json");
        public static SystemSetting Setting { get; private set; }

        static AppConfig()
        {
            Load();
        }

        public static void Load()
        {
            try
            {
                if (File.Exists(ConfigPath))
                {
                    string json = File.ReadAllText(ConfigPath);
                    var setting = new JavaScriptSerializer().Deserialize<SystemSetting>(json);
                    if (setting != null)
                    {
                        if (string.IsNullOrEmpty(setting.PhotoRootPath)) setting.PhotoRootPath = @"C:\Photo";
                        if (string.IsNullOrEmpty(setting.UploadPath)) setting.UploadPath = @"C:\Upload";
                        Setting = setting;
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error("載入 app_settings.json 失敗，使用預設值", ex);
            }

            // 若尚未存在 app_settings.json，以預設值建立並儲存檔案
            Logger.Info("未找到或無法載入 app_settings.json，正在以預設值自動建立...");
            Setting = new SystemSetting
            {
                LineName = ConfigurationManager.AppSettings["LineName"] ?? "FQC_1",
                PhotoRootPath = ConfigurationManager.AppSettings["PhotoRootPath"] ?? @"C:\Photo",
                MaxPhotoCount = int.TryParse(ConfigurationManager.AppSettings["MaxPhotoCount"], out int max) ? max : 5,
                DesiredWidth = int.TryParse(ConfigurationManager.AppSettings["DesiredWidth"], out int width) ? width : 3840,
                UploadPath = ConfigurationManager.AppSettings["UploadPath"] ?? @"C:\Upload"
            };

            Save();
        }

        public static void Save()
        {
            try
            {
                string json = new JavaScriptSerializer().Serialize(Setting);
                File.WriteAllText(ConfigPath, json);
                Logger.Info("系統設定已儲存至: " + ConfigPath);
            }
            catch (Exception ex)
            {
                Logger.Error("儲存 app_settings.json 失敗", ex);
            }
        }

        public static void UpdateSettings(string lineName, string photoRootPath, int desiredWidth, string uploadPath)
        {
            Setting.LineName = lineName;
            Setting.PhotoRootPath = photoRootPath;
            Setting.DesiredWidth = desiredWidth;
            Setting.UploadPath = uploadPath;
            Save();
        }

        public static string LineName => Setting?.LineName ?? "FQC_1";
        public static string PhotoRootPath => Setting?.PhotoRootPath ?? @"C:\Photo";
        public static int MaxPhotoCount => Setting?.MaxPhotoCount ?? 5;
        public static int DesiredWidth => Setting?.DesiredWidth ?? 3840;
        public static string UploadPath => Setting?.UploadPath ?? @"C:\Upload";

        public static int CameraIndex => int.TryParse(ConfigurationManager.AppSettings["CameraIndex"], out int idx) ? idx : 0;
        public static string ScannerComPort => ConfigurationManager.AppSettings["ScannerComPort"] ?? "COM5";
        public static int ScannerBaudRate => int.TryParse(ConfigurationManager.AppSettings["ScannerBaudRate"], out int baud) ? baud : 115200;
    }
}
