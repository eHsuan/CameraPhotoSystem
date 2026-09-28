using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace CameraPhotoSystem.Utils
{
    public static class IconHelper
    {
        private static Icon _cachedIcon;
        private static bool _initialized = false;
        private static readonly object _lock = new object();

        /// <summary>
        /// 取得系統預設圖示（CameraPhotoSystem.ico）
        /// </summary>
        public static Icon GetAppIcon()
        {
            if (_initialized)
            {
                return _cachedIcon;
            }

            lock (_lock)
            {
                if (_initialized)
                {
                    return _cachedIcon;
                }

                try
                {
                    // 1. 優先從執行檔同目錄尋找 CameraPhotoSystem.ico
                    string iconPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "CameraPhotoSystem.ico");
                    if (File.Exists(iconPath))
                    {
                        _cachedIcon = new Icon(iconPath);
                    }
                    else
                    {
                        // 2. 若檔案不存在，從執行檔提取內嵌之關聯圖示
                        _cachedIcon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error("載入應用程式圖示失敗", ex);
                }
                finally
                {
                    _initialized = true;
                }

                return _cachedIcon;
            }
        }

        /// <summary>
        /// 套用應用程式圖示至指定的視窗
        /// </summary>
        public static void ApplyFormIcon(Form form)
        {
            if (form == null || form.IsDisposed) return;

            try
            {
                var icon = GetAppIcon();
                if (icon != null)
                {
                    form.Icon = icon;
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"套用視窗圖示失敗: {form.Name}", ex);
            }
        }
    }
}
