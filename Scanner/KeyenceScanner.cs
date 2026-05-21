using System;
using System.IO.Ports;
using System.Text;
using CameraPhotoSystem.Utils;

namespace CameraPhotoSystem.Scanner
{
    public class KeyenceScanner : IDisposable
    {
        private SerialPort _serialPort;
        private StringBuilder _buffer = new StringBuilder();
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
                
                // 開啟 RTS 與 DTR，這是工業級讀碼器正常傳輸資料的關鍵
                _serialPort.RtsEnable = true;
                _serialPort.DtrEnable = true;
                
                // 設定讀取逾時與結束符號
                _serialPort.ReadTimeout = 500;
                _serialPort.NewLine = "\r";

                _serialPort.DataReceived += SerialPort_DataReceived;
                _serialPort.Open();
                
                _buffer.Clear();
                Logger.Info(string.Format("Scanner connected on {0} (Baud:{1}, RTS/DTR:Enabled)", portName, baudRate));

                // 連線成功後立即送出 LON 指令啟動讀碼
                SendCommand("LON");
            }
            catch (Exception ex)
            {
                Logger.Error("Failed to start scanner", ex);
                throw;
            }
        }

        public void SendCommand(string cmd)
        {
            try
            {
                if (_serialPort != null && _serialPort.IsOpen)
                {
                    _serialPort.Write(cmd + "\r");
                    Logger.Info("Scanner command sent: " + cmd);
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Failed to send command: " + cmd, ex);
            }
        }

        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (_serialPort == null || !_serialPort.IsOpen) return;

            try
            {
                string incoming = _serialPort.ReadExisting();
                if (string.IsNullOrEmpty(incoming)) return;

                _buffer.Append(incoming);
                string currentContent = _buffer.ToString();

                if (currentContent.Contains("\r") || currentContent.Contains("\n"))
                {
                    string[] parts = currentContent.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                    
                    foreach (var rawBarcode in parts)
                    {
                        string barcode = rawBarcode.Trim();
                        if (string.IsNullOrEmpty(barcode) || barcode == "ERROR" || barcode.Length < 3) continue;

                        Logger.Info("Scanner received: " + barcode);
                        OnBarcodeScanned?.Invoke(barcode);
                    }

                    int lastNewLine = currentContent.LastIndexOfAny(new[] { '\r', '\n' });
                    if (lastNewLine >= 0 && lastNewLine < currentContent.Length - 1)
                    {
                        _buffer.Clear();
                        _buffer.Append(currentContent.Substring(lastNewLine + 1));
                    }
                    else
                    {
                        _buffer.Clear();
                    }
                }
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
                if (_serialPort.IsOpen)
                {
                    // 關閉前送出 LOFF 指令停止讀碼
                    SendCommand("LOFF");
                    System.Threading.Thread.Sleep(100); // 確保指令送出
                    _serialPort.Close();
                }
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