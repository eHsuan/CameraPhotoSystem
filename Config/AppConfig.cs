using System;
using System.Configuration;

namespace CameraPhotoSystem.Config
{
    public static class AppConfig
    {
        public static string PhotoRootPath { get { return ConfigurationManager.AppSettings["PhotoRootPath"] ?? @"D:\Photo"; } }
        public static int CameraIndex { get { return int.Parse(ConfigurationManager.AppSettings["CameraIndex"] ?? "0"); } }
        public static int MaxPhotoCount { get { return int.Parse(ConfigurationManager.AppSettings["MaxPhotoCount"] ?? "5"); } }
        public static string DatabasePath { get { return ConfigurationManager.AppSettings["DatabasePath"] ?? "CameraData.db"; } }
        public static int DesiredWidth { get { return int.Parse(ConfigurationManager.AppSettings["DesiredWidth"] ?? "3840"); } }
        public static string ScannerComPort { get { return ConfigurationManager.AppSettings["ScannerComPort"] ?? "COM5"; } }
        public static int ScannerBaudRate { get { return int.Parse(ConfigurationManager.AppSettings["ScannerBaudRate"] ?? "115200"); } }
    }
}
