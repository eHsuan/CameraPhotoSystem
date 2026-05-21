using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Script.Serialization;

namespace CameraPhotoSystem.Config
{
    public class CameraSetting
    {
        public string MonikerString { get; set; } = "";
        public int FocusValue { get; set; } = 40;
        public bool AutoFocus { get; set; } = false;
        public int ExposureValue { get; set; } = -5;
        public bool AutoExposure { get; set; } = false;
    }

    public static class CameraConfigManager
    {
        private static string ConfigPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "camera_settings.json");
        public static List<CameraSetting> Settings { get; private set; }

        static CameraConfigManager()
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
                    var list = new JavaScriptSerializer().Deserialize<List<CameraSetting>>(json);
                    if (list != null && list.Count >= 3)
                    {
                        Settings = list;
                        return;
                    }
                }
            }
            catch (Exception ex) { 
                Utils.Logger.Error("載入相機設定失敗", ex);
            }

            // 預設值
            Settings = new List<CameraSetting>
            {
                new CameraSetting(),
                new CameraSetting { FocusValue = 13 }, 
                new CameraSetting()
            };
        }

        public static void Save()
        {
            try
            {
                string json = new JavaScriptSerializer().Serialize(Settings);
                File.WriteAllText(ConfigPath, json);
                Utils.Logger.Info("相機設定已儲存至: " + ConfigPath);
            }
            catch (Exception ex) {
                Utils.Logger.Error("儲存相機設定失敗", ex);
            }
        }
    }
}