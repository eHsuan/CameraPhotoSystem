using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;
using AForge.Video;
using AForge.Video.DirectShow;
using CameraPhotoSystem.Config;
using CameraPhotoSystem.Utils;

namespace CameraPhotoSystem.Camera
{
    public class CameraManager
    {
        private List<FilterInfo> _videoDevices;
        private VideoCaptureDevice[] _videoSources = new VideoCaptureDevice[3];
        private VideoCaptureDevice _testSource = null;
        private Bitmap[] _currentFrames = new Bitmap[3];
        private readonly object[] _frameLocks = new object[3];
        private int _activePreviewIndex = -1;
        private bool _isPreviewingTest = false;

        public event Action<object, Bitmap> OnFrameArrived;

        public CameraManager()
        {
            for (int i = 0; i < 3; i++) _frameLocks[i] = new object();
        }

        private void RefreshVideoDevices()
        {
            var devices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            _videoDevices = new List<FilterInfo>();
            foreach (FilterInfo device in devices)
            {
                string name = device.Name.ToUpper();
                if (name.Contains("MX BRIO") || name.Contains("BRIO") || name.Contains("LOGITECH"))
                {
                    _videoDevices.Add(device);
                }
            }
        }

        public int DeviceCount { get { RefreshVideoDevices(); return _videoDevices.Count; } }
        public List<FilterInfo> VideoDevices { get { RefreshVideoDevices(); return _videoDevices; } }

        public void StartAllCameras()
        {
            RefreshVideoDevices();
            StopAllCameras();

            var availableDevices = new List<FilterInfo>(_videoDevices);
            var assignedDevices = new FilterInfo[3];

            for (int i = 0; i < 3; i++)
            {
                if (i < CameraConfigManager.Settings.Count)
                {
                    string savedMoniker = CameraConfigManager.Settings[i].MonikerString;
                    if (!string.IsNullOrEmpty(savedMoniker))
                    {
                        var match = availableDevices.Find(d => d.MonikerString == savedMoniker);
                        if (match != null)
                        {
                            assignedDevices[i] = match;
                            availableDevices.Remove(match);
                            Logger.Info(string.Format("相機 {0} 匹配成功: {1}", i + 1, match.Name));
                        }
                        else
                        {
                            Logger.Info(string.Format("相機 {0} 儲存的編號未找到，將自動分配", i + 1));
                        }
                    }
                }
            }

            bool settingsChanged = false;
            for (int i = 0; i < 3; i++)
            {
                if (assignedDevices[i] == null && availableDevices.Count > 0)
                {
                    assignedDevices[i] = availableDevices[0];
                    availableDevices.RemoveAt(0);

                    if (i < CameraConfigManager.Settings.Count)
                    {
                        CameraConfigManager.Settings[i].MonikerString = assignedDevices[i].MonikerString;
                        settingsChanged = true;
                        Logger.Info(string.Format("相機 {0} 自動分配為: {1}", i + 1, assignedDevices[i].Name));
                    }
                }
            }

            if (settingsChanged) CameraConfigManager.Save();

            for (int i = 0; i < 3; i++)
            {
                if (assignedDevices[i] == null) continue;

                int cameraIndex = i;
                try
                {
                    VideoCaptureDevice source = new VideoCaptureDevice(assignedDevices[i].MonikerString);
                    
                    if (source.VideoCapabilities != null && source.VideoCapabilities.Length > 0)
                    {
                        int targetWidth = AppConfig.DesiredWidth;
                        VideoCapabilities bestCap = source.VideoCapabilities[0];
                        
                        // 記錄所有可用的解析度以供調試
                        foreach (var cap in source.VideoCapabilities)
                        {
                            Logger.Info(string.Format("相機 {0} 可用解析度: {1}x{2}@{3}fps", 
                                cameraIndex + 1, cap.FrameSize.Width, cap.FrameSize.Height, cap.AverageFrameRate));
                        }

                        // 尋找最接近目標寬度的解析度，且優先選擇幀率較高的
                        foreach (var cap in source.VideoCapabilities)
                        {
                            if (cap.FrameSize.Width == targetWidth)
                            {
                                if (bestCap.FrameSize.Width != targetWidth || cap.AverageFrameRate > bestCap.AverageFrameRate)
                                {
                                    bestCap = cap;
                                }
                            }
                            else if (bestCap.FrameSize.Width != targetWidth)
                            {
                                // 如果還沒找到目標寬度，則保留目前最大的寬度
                                if (cap.FrameSize.Width > bestCap.FrameSize.Width)
                                {
                                    bestCap = cap;
                                }
                                else if (cap.FrameSize.Width == bestCap.FrameSize.Width && cap.AverageFrameRate > bestCap.AverageFrameRate)
                                {
                                    bestCap = cap;
                                }
                            }
                        }

                        source.VideoResolution = bestCap;
                        Logger.Info(string.Format("相機 {0} 最終選定解析度: {1}x{2}", cameraIndex + 1, bestCap.FrameSize.Width, bestCap.FrameSize.Height));
                    }

                    source.NewFrame += (s, e) => OnNewFrame(cameraIndex, e);
                    _videoSources[cameraIndex] = source;
                    source.Start();
                    
                    System.Threading.Thread.Sleep(500); 
                    Logger.Info(string.Format("相機 {0} 啟動指令已發送: {1}", i + 1, assignedDevices[i].Name));
                }
                catch (Exception ex) { Logger.Error(string.Format("相機 {0} 啟動失敗", i + 1), ex); }
            }
        }

        public void PreviewHardware(string moniker)
        {
            StopTestSource();
            
            // 重要：檢查是否有背景相機正在佔用此硬體，若有則先停止它
            for (int i = 0; i < 3; i++)
            {
                if (_videoSources[i] != null && _videoSources[i].Source == moniker)
                {
                    Logger.Info(string.Format("預覽衝突：停止相機 {0} 以釋放硬體資源", i + 1));
                    try { _videoSources[i].SignalToStop(); _videoSources[i] = null; } catch { }
                }
            }

            _isPreviewingTest = true;
            try
            {
                _testSource = new VideoCaptureDevice(moniker);
                if (_testSource.VideoCapabilities != null && _testSource.VideoCapabilities.Length > 0)
                {
                    int targetWidth = AppConfig.DesiredWidth;
                    VideoCapabilities bestCap = _testSource.VideoCapabilities[0];
                    bool found = false;

                    foreach (var cap in _testSource.VideoCapabilities)
                    {
                        if (cap.FrameSize.Width == targetWidth)
                        {
                            bestCap = cap;
                            found = true;
                            break;
                        }
                    }

                    if (!found)
                    {
                        foreach (var cap in _testSource.VideoCapabilities)
                        {
                            if (cap.FrameSize.Width <= targetWidth && cap.FrameSize.Width > bestCap.FrameSize.Width)
                                bestCap = cap;
                        }
                    }
                    _testSource.VideoResolution = bestCap;
                }
                _testSource.NewFrame += (s, e) => {
                    if (_isPreviewingTest && OnFrameArrived != null)
                    {
                        OnFrameArrived.Invoke(this, (Bitmap)e.Frame.Clone());
                    }
                };
                _testSource.Start();
            }
            catch { _isPreviewingTest = false; }
        }

        public void StopTestSource()
        {
            _isPreviewingTest = false;
            if (_testSource != null)
            {
                try { _testSource.SignalToStop(); } catch { }
                _testSource = null;
            }
        }

        #region 硬體參數控制 (焦距、曝光)

        private IAMCameraControl GetCameraControl(int index)
        {
            if (index < 0 || index >= 3 || _videoSources[index] == null) return null;
            try
            {
                var source = _videoSources[index];
                var field = typeof(VideoCaptureDevice).GetField("sourceFilter", BindingFlags.Instance | BindingFlags.NonPublic);
                return field?.GetValue(source) as IAMCameraControl;
            }
            catch { return null; }
        }

        public void SetFocus(int index, int value)
        {
            var control = GetCameraControl(index);
            control?.Set(CameraControlProperty.Focus, value, CameraControlFlags.Manual);
        }

        public bool GetFocusRange(int index, out int min, out int max, out int step, out int defaultValue)
        {
            min = 0; max = 100; step = 1; defaultValue = 0;
            var control = GetCameraControl(index);
            if (control == null) return false;
            CameraControlFlags caps;
            return control.GetRange(CameraControlProperty.Focus, out min, out max, out step, out defaultValue, out caps) >= 0;
        }

        public void SetExposure(int index, int value, bool auto)
        {
            var control = GetCameraControl(index);
            control?.Set(CameraControlProperty.Exposure, value, auto ? CameraControlFlags.Auto : CameraControlFlags.Manual);
        }

        public void ShowProperties(int index, IntPtr parentHandle)
        {
            // 優先顯示目前正在「測試預覽」的源
            if (_isPreviewingTest && _testSource != null)
            {
                try { _testSource.DisplayPropertyPage(parentHandle); return; } catch { }
            }

            // 如果沒有測試預覽，則顯示目前主畫面上正在預覽的邏輯相機源
            int activeIdx = (_activePreviewIndex >= 0) ? _activePreviewIndex : index;
            if (activeIdx >= 0 && activeIdx < 3 && _videoSources[activeIdx] != null)
            {
                try { _videoSources[activeIdx].DisplayPropertyPage(parentHandle); } catch { }
            }
        }

        #endregion

        private void OnNewFrame(int index, NewFrameEventArgs eventArgs)
        {
            if (_isPreviewingTest) return; 

            Bitmap frameToDispose = null;
            try
            {
                Bitmap newFrame = (Bitmap)eventArgs.Frame.Clone();

                lock (_frameLocks[index])
                {
                    frameToDispose = _currentFrames[index];
                    _currentFrames[index] = newFrame;
                }

                if (index == _activePreviewIndex && OnFrameArrived != null)
                {
                    OnFrameArrived.Invoke(this, (Bitmap)newFrame.Clone());
                }
            }
            catch { }
            finally
            {
                if (frameToDispose != null)
                {
                    try { frameToDispose.Dispose(); } catch { }
                }
            }
        }

        public void SetPreviewIndex(int index) 
        { 
            _activePreviewIndex = index; 
            StopTestSource(); // 切換預覽索引時，務必停止測試模式

            // 立即發送當前相機的最後一幀，讓 UI 切換更有感
            if (index >= 0 && index < 3)
            {
                lock (_frameLocks[index])
                {
                    if (_currentFrames[index] != null && OnFrameArrived != null)
                    {
                        try { OnFrameArrived.Invoke(this, (Bitmap)_currentFrames[index].Clone()); } catch { }
                    }
                }
            }
        }

        public void StopAllCameras()
        {
            StopTestSource();
            for (int i = 0; i < 3; i++)
            {
                if (_videoSources[i] != null)
                {
                    try { _videoSources[i].SignalToStop(); } catch { }
                    _videoSources[i] = null;
                }
            }
            for (int i = 0; i < 3; i++) lock (_frameLocks[i]) { if (_currentFrames[i] != null) { _currentFrames[i].Dispose(); _currentFrames[i] = null; } }
        }

        public Bitmap CaptureImageFromIndex(int index, int timeoutMs = 4000)
        {
            if (index < 0 || index >= 3 || _videoSources[index] == null)
                throw new Exception(string.Format("相機 {0} 尚未初始化或啟動失敗。", index + 1));

            var start = DateTime.Now;
            while ((DateTime.Now - start).TotalMilliseconds < timeoutMs)
            {
                lock (_frameLocks[index]) 
                { 
                    if (_currentFrames[index] != null) return (Bitmap)_currentFrames[index].Clone(); 
                }
                System.Threading.Thread.Sleep(100);
            }

            throw new Exception(string.Format("相機 {0} 影像抓取逾時。", index + 1));
        }

        #region DirectShow COM Interop
        [ComImport, Guid("C6E13070-3071-11D0-AD8B-00C04F0E5746"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        public interface IAMCameraControl
        {
            [PreserveSig] int GetRange([In] CameraControlProperty Property, [Out] out int pMin, [Out] out int pMax, [Out] out int pSteppingDelta, [Out] out int pDefault, [Out] out CameraControlFlags pCapsFlags);
            [PreserveSig] int Set([In] CameraControlProperty Property, [In] int lValue, [In] CameraControlFlags Flags);
            [PreserveSig] int Get([In] CameraControlProperty Property, [Out] out int lValue, [Out] out CameraControlFlags pFlags);
        }

        public enum CameraControlProperty { Pan = 0, Tilt, Roll, Zoom, Exposure, Iris, Focus }
        [Flags] public enum CameraControlFlags { None = 0, Manual = 1, Auto = 2 }
        #endregion
    }
}