using System;
using System.IO;
using System.Web.Script.Serialization;

namespace CameraPhotoSystem.Config
{
    public class ScannerSetting
    {
        public string PortName { get; set; } = "COM3";
        public int BaudRate { get; set; } = 9600;
    }

    public static class ScannerConfigManager
    {
        private static string ConfigPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "scanner_config.json");
        public static ScannerSetting Setting { get; private set; }

        static ScannerConfigManager()
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
                    Setting = new JavaScriptSerializer().Deserialize<ScannerSetting>(json);
                    if (Setting != null) return;
                }
            }
            catch { }
            Setting = new ScannerSetting();
        }

        public static void Save()
        {
            try
            {
                string json = new JavaScriptSerializer().Serialize(Setting);
                File.WriteAllText(ConfigPath, json);
            }
            catch { }
        }
    }
}
