using System;
using System.IO.Ports;
using System.Text;
using CameraPhotoSystem.Utils;

namespace CameraPhotoSystem.Scanner
{
    public class KeyenceScanner : IDisposable
    {
        private SerialPort _serialPort;
        public event Action<string> OnBarcodeScanned;
        
        public bool IsConnected => _serialPort != null && _serialPort.IsOpen;

        public void Start(string portName, int baudRate)
        {
            try
            {
                if (_serialPort != null)
                {
                    Stop();
                }

                _serialPort = new SerialPort(portName, baudRate, Parity.None, 8, StopBits.One);
                _serialPort.DataReceived += SerialPort_DataReceived;
                _serialPort.Open();
                Logger.Info(string.Format("Scanner connected on {0}", portName));
            }
            catch (Exception ex)
            {
                Logger.Error("Failed to start scanner", ex);
                throw;
            }
        }

        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                if (_serialPort == null || !_serialPort.IsOpen) return;

                // 讀取現有資料
                string data = _serialPort.ReadExisting();
                if (string.IsNullOrEmpty(data)) return;

                // Keyence 預設可能會帶 \r 或 \n，進行清理
                string barcode = data.Replace("\r", "").Replace("\n", "").Trim();
                
                // 過濾掉可能的錯誤訊息
                if (barcode == "ERROR" || barcode.Length < 3) return;

                OnBarcodeScanned?.Invoke(barcode);
            }
            catch (Exception ex)
            {
                Logger.Error("Error reading scanner data", ex);
            }
        }

        public void Stop()
        {
            if (_serialPort != null)
            {
                if (_serialPort.IsOpen) _serialPort.Close();
                _serialPort.Dispose();
                _serialPort = null;
            }
        }

        public void Dispose()
        {
            Stop();
        }
    }
}