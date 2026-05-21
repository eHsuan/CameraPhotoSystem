using System;
using System.Windows.Forms;
using CameraPhotoSystem.UI;

namespace CameraPhotoSystem
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            // 初始化 log4net 讀取 App.config 設定
            log4net.Config.XmlConfigurator.Configure();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}