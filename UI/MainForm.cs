using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using CameraPhotoSystem.Camera;
using CameraPhotoSystem.Config;
using CameraPhotoSystem.Service;
using CameraPhotoSystem.Utils;

namespace CameraPhotoSystem.UI
{
    public partial class MainForm : Form
    {
        private readonly CameraManager _cameraManager;
        private readonly CaptureService _captureService;
        private readonly ApiService _apiService;
        private Scanner.KeyenceScanner _scanner;
        
        private int _currentPhotoIndex = 0;
        private bool _isProcessingFrame = false;
        private bool _isUpdatingUI = false;
        private System.Collections.Generic.List<AForge.Video.DirectShow.FilterInfo> _cachedDevices;
        
        private string _lastFinishedDmc = string.Empty;
        private bool _isLocked = false;

        private System.Collections.Generic.List<ComboBox> _mappingDropdowns = new System.Collections.Generic.List<ComboBox>();
        private System.Collections.Generic.List<Label> _mappingLabels = new System.Collections.Generic.List<Label>();

        public MainForm()
        {
            InitializeComponent();
            this.Font = new Font("Segoe UI", 9F); 
            this.WindowState = FormWindowState.Maximized; 
            
            var prop = typeof(Control).GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            if (prop != null) prop.SetValue(pbCamera, true, null);

            _cameraManager = new CameraManager();
            _captureService = new CaptureService(_cameraManager);
            _apiService = new ApiService();
            
            _cameraManager.OnFrameArrived += (s, bmp) => {
                if (this.IsDisposed || _isProcessingFrame) { bmp.Dispose(); return; }
                _isProcessingFrame = true;
                this.BeginInvoke(new Action(() => {
                    try {
                        if (this.IsDisposed) { bmp.Dispose(); return; }
                        var oldImg = pbCamera.Image;
                        pbCamera.Image = bmp;
                        if (oldImg != null) oldImg.Dispose();
                    } catch { bmp.Dispose(); } finally { _isProcessingFrame = false; }
                }));
            };

            numPhotoCount.Value = 0;
            numPhotoCount.Maximum = AppConfig.MaxPhotoCount;
            numPhotoCount.Enabled = false; 
            txtDataMatrix.TextChanged += txtDataMatrix_TextChanged;
        }

        private async void MainForm_Load(object sender, EventArgs e)
        {
            try {
                InitHardwareReorderUI();
                cmbLanguage.SelectedIndex = (L.Current == Language.CH) ? 0 : 1;
                UpdateUILanguage();

                AddLog(L.T("LogStart"));
                _cameraManager.StartAllCameras();
                await Task.Delay(1500);

                int count = _cameraManager.DeviceCount;
                AddLog(string.Format(L.T("LogDetected"), count));
                
                _cameraManager.SetPreviewIndex(0);
                AddLog(L.T("LogSystemReady"));

                InitScanner();
            } catch (Exception ex) {
                AddLog("Error: " + ex.Message);
            }
        }

        private void InitScanner()
        {
            try
            {
                if (_scanner != null) _scanner.Stop();
                _scanner = new Scanner.KeyenceScanner();
                _scanner.OnBarcodeScanned += (barcode) => {
                    this.BeginInvoke(new Action(() => { HandleScannerInput(barcode); }));
                };
                
                string port = ScannerConfigManager.Setting.PortName;
                int baud = ScannerConfigManager.Setting.BaudRate;
                _scanner.Start(port, baud);
                AddLog(string.Format("掃描器已啟動於 {0} (包率: {1})", port, baud));
            }
            catch (Exception ex) { AddLog("掃描器連線失敗: " + ex.Message); }
        }

        private void HandleScannerInput(string newBarcode)
        {
            string currentInText = txtDataMatrix.Text.Trim();
            if (newBarcode == currentInText) return;

            if (newBarcode == _lastFinishedDmc)
            {
                _isLocked = true;
                txtDataMatrix.Text = newBarcode;
                txtDataMatrix.BackColor = Color.Red;
                btnCapture.Enabled = false;
                btnCapture.Text = (L.Current == Language.CH) ? "連續重複(鎖定)" : "Duplicate Lock";
                AddLog("【警告】連續條碼重複，系統已鎖定。");
                return;
            }

            _isLocked = false;
            _currentPhotoIndex = 0;
            numPhotoCount.Value = 0;
            txtDataMatrix.Text = newBarcode;
            txtDataMatrix.BackColor = SystemColors.Window;
            
            AddLog("掃描成功: " + newBarcode);
            UpdateCaptureButtonUI();
            UpdatePreviewBasedOnProgress();
        }

        private void InitHardwareReorderUI()
        {
            // 1. 隱藏設計介面原本多餘的元件
            if (lblEngCam != null) lblEngCam.Visible = false;
            if (cmbEngCameraSelect != null) cmbEngCameraSelect.Visible = false;

            // 2. 清空工程分頁並重新垂直堆疊佈局
            tpEngineering.Controls.Clear();
            
            // --- 群組 1: 相機映射 (最上方) ---
            grpEngControl.Controls.Clear();
            grpEngControl.Text = (L.Current == Language.CH) ? "相機順序映射設定" : "Camera Mapping";
            grpEngControl.Location = new Point(6, 10);
            grpEngControl.Size = new Size(268, 430);
            tpEngineering.Controls.Add(grpEngControl);

            _mappingDropdowns.Clear();
            _mappingLabels.Clear();
            _cachedDevices = _cameraManager.VideoDevices;
            List<string> displayNames = new List<string>();
            for (int i = 0; i < _cachedDevices.Count; i++) {
                var d = _cachedDevices[i];
                string shortId = d.MonikerString.Contains("#") ? d.MonikerString.Split('#')[1] : "ID-" + i;
                if (shortId.Length > 8) shortId = shortId.Substring(0, 8);
                displayNames.Add(string.Format("[裝置 {0}] {1} (ID: {2})", i + 1, d.Name, shortId));
            }

            for (int i = 0; i < 3; i++) {
                int camIdx = i;
                Label lbl = new Label() {
                    Text = string.Format((L.Current == Language.CH) ? "邏輯相機 {0} 號：" : "Logical Cam {0}:", camIdx + 1),
                    Location = new Point(10, 30 + (i * 75)), Size = new Size(240, 25), Font = new Font("Segoe UI", 9, FontStyle.Bold)
                };
                ComboBox cmb = new ComboBox() {
                    DropDownStyle = ComboBoxStyle.DropDownList, Location = new Point(10, 55 + (i * 75)),
                    Size = new Size(170, 30), Font = new Font("Segoe UI", 11), Tag = camIdx
                };
                Button btnPrev = new Button() {
                    Text = (L.Current == Language.CH) ? "預覽" : "Prev", Location = new Point(185, 54 + (i * 75)),
                    Size = new Size(65, 32), BackColor = Color.LightSteelBlue, Font = new Font("Segoe UI", 9)
                };
                foreach (var name in displayNames) cmb.Items.Add(name);
                if (camIdx < CameraConfigManager.Settings.Count) {
                    int mIdx = _cachedDevices.FindIndex(d => d.MonikerString == CameraConfigManager.Settings[camIdx].MonikerString);
                    if (mIdx >= 0) cmb.SelectedIndex = mIdx;
                }

                Action updateHighlight = () => {
                    for(int j=0; j<_mappingLabels.Count; j++) {
                        bool isActive = (j == camIdx);
                        _mappingLabels[j].ForeColor = isActive ? Color.Green : SystemColors.ControlText;
                        string baseText = string.Format((L.Current == Language.CH) ? "邏輯相機 {0} 號" : "Logical Cam {0}", j + 1);
                        _mappingLabels[j].Text = isActive ? baseText + " (預覽中)" : baseText + "：";
                    }
                };

                cmb.SelectedIndexChanged += (s, e) => { if (!_isUpdatingUI && cmb.SelectedIndex >= 0) { _cameraManager.PreviewHardware(_cachedDevices[cmb.SelectedIndex].MonikerString); updateHighlight(); } };
                btnPrev.Click += (s, e) => { if (cmb.SelectedIndex >= 0) { _cameraManager.PreviewHardware(_cachedDevices[cmb.SelectedIndex].MonikerString); updateHighlight(); } };

                _mappingDropdowns.Add(cmb); _mappingLabels.Add(lbl);
                grpEngControl.Controls.Add(lbl); grpEngControl.Controls.Add(cmb); grpEngControl.Controls.Add(btnPrev);
            }

            Button btnApplyCam = new Button() {
                Text = (L.Current == Language.CH) ? "儲存並套用新順序" : "Apply Cam Settings",
                Location = new Point(10, 260), Size = new Size(240, 45), BackColor = Color.LightGreen, Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            btnApplyCam.Click += (s, e) => {
                for (int i = 0; i < 3; i++) if (_mappingDropdowns[i].SelectedIndex >= 0) CameraConfigManager.Settings[i].MonikerString = _cachedDevices[_mappingDropdowns[i].SelectedIndex].MonikerString;
                CameraConfigManager.Save(); _cameraManager.StopAllCameras(); System.Threading.Thread.Sleep(500); _cameraManager.StartAllCameras(); _cameraManager.SetPreviewIndex(0);
                MessageBox.Show((L.Current == Language.CH) ? "相機設定已生效！" : "Camera settings applied!");
            };

            Button btnProp = new Button() {
                Text = (L.Current == Language.CH) ? "開啟目前預覽裝置之屬性" : "Show Properties",
                Location = new Point(10, 315), Size = new Size(240, 40), Font = new Font("Segoe UI", 9)
            };
            btnProp.Click += (s, e) => _cameraManager.ShowProperties(0, this.Handle);

            Label lblHint = new Label() { Text = L.T("LblEngHint"), Location = new Point(10, 365), Size = new Size(240, 50), ForeColor = Color.Red, Font = new Font("Segoe UI", 8) };
            grpEngControl.Controls.Add(btnApplyCam); grpEngControl.Controls.Add(btnProp); grpEngControl.Controls.Add(lblHint);

            // --- 群組 2: 掃描器設定 (接在相機映射下方) ---
            GroupBox grpScanner = new GroupBox() {
                Text = (L.Current == Language.CH) ? "掃描器連線設定" : "Scanner Settings",
                Location = new Point(6, 445),
                Size = new Size(268, 240),
                Font = new Font("Segoe UI", 9)
            };
            tpEngineering.Controls.Add(grpScanner);

            Label lblPort = new Label() { Text = "COM Port:", Location = new Point(15, 30), Size = new Size(100, 20) };
            ComboBox cmbPort = new ComboBox() { DropDownStyle = ComboBoxStyle.DropDownList, Location = new Point(15, 50), Size = new Size(230, 30), Font = new Font("Segoe UI", 11) };
            try {
                string[] ports = System.IO.Ports.SerialPort.GetPortNames();
                foreach (var p in ports) cmbPort.Items.Add(p);
                if (cmbPort.Items.Contains(ScannerConfigManager.Setting.PortName)) cmbPort.SelectedItem = ScannerConfigManager.Setting.PortName;
                else if (cmbPort.Items.Count > 0) cmbPort.SelectedIndex = 0;
            } catch { }

            Label lblBaud = new Label() { Text = (L.Current == Language.CH) ? "包率 (Baud Rate):" : "Baud Rate:", Location = new Point(15, 100), Size = new Size(150, 20) };
            ComboBox cmbBaud = new ComboBox() { DropDownStyle = ComboBoxStyle.DropDownList, Location = new Point(15, 120), Size = new Size(230, 30), Font = new Font("Segoe UI", 11) };
            int[] bauds = { 4800, 9600, 19200, 38400, 57600, 115200 };
            foreach (var b in bauds) cmbBaud.Items.Add(b);
            cmbBaud.SelectedItem = ScannerConfigManager.Setting.BaudRate;
            if (cmbBaud.SelectedIndex < 0) cmbBaud.SelectedIndex = 1;

            Button btnApplyScanner = new Button() {
                Text = (L.Current == Language.CH) ? "儲存並重啟掃描器" : "Save & Restart Scanner",
                Location = new Point(15, 180), Size = new Size(230, 45), BackColor = Color.LightYellow, Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };
            btnApplyScanner.Click += (s, e) => {
                if (cmbPort.SelectedItem == null) return;
                ScannerConfigManager.Setting.PortName = cmbPort.SelectedItem.ToString();
                ScannerConfigManager.Setting.BaudRate = (int)cmbBaud.SelectedItem;
                ScannerConfigManager.Save(); InitScanner();
                MessageBox.Show((L.Current == Language.CH) ? "掃描器設定已儲存並重啟" : "Scanner settings saved and restarted");
            };
            grpScanner.Controls.Add(lblPort); grpScanner.Controls.Add(cmbPort);
            grpScanner.Controls.Add(lblBaud); grpScanner.Controls.Add(cmbBaud);
            grpScanner.Controls.Add(btnApplyScanner);

            // --- 語系設定 (最下方) ---
            tpEngineering.Controls.Add(grpLang);
            grpLang.Location = new Point(6, 690);
        }

        private void UpdateUILanguage()
        {
            this.SuspendLayout();
            try {
                this.Text = L.T("WindowTitle");
                tpProduction.Text = L.T("TabProduction");
                tpEngineering.Text = L.T("TabEngineering");
                lblDM.Text = L.T("LblScan");
                lblCount.Text = L.T("LblProgress");
                btnOpenQuery.Text = L.T("BtnQuery");
                InitHardwareReorderUI();
                UpdateCaptureButtonUI();
            } finally { this.ResumeLayout(); }
        }

        private void cmbLanguage_SelectedIndexChanged(object sender, EventArgs e)
        {
            L.Current = (cmbLanguage.SelectedIndex == 0) ? Language.CH : Language.DE;
            UpdateUILanguage();
        }

        private void tabMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabMain.SelectedTab == tpEngineering)
            {
                using (var prompt = new Form() { Width = 350, Height = 160, Text = "Auth", StartPosition = FormStartPosition.CenterParent, Font = new Font("Segoe UI", 9) })
                {
                    Label textLabel = new Label() { Left = 20, Top = 20, Text = L.T("MsgPassword"), Width = 250 };
                    TextBox textBox = new TextBox() { Left = 20, Top = 50, Width = 280, PasswordChar = '*' };
                    Button confirmation = new Button() { Text = "OK", Left = 220, Width = 80, Top = 85, DialogResult = DialogResult.OK };
                    prompt.Controls.Add(textBox); prompt.Controls.Add(confirmation); prompt.Controls.Add(textLabel);
                    prompt.AcceptButton = confirmation;

                    if (prompt.ShowDialog() == DialogResult.OK && textBox.Text == "8888") { _cameraManager.SetPreviewIndex(0); }
                    else { tabMain.SelectedTab = tpProduction; if (textBox.Text != "") MessageBox.Show(L.T("MsgWrongPwd")); }
                }
            }
            else { AddLog("正在從工程模式恢復生產連線..."); _cameraManager.StartAllCameras(); UpdatePreviewBasedOnProgress(); }
        }

        private void cmbEngCameraSelect_SelectedIndexChanged(object sender, EventArgs e) { }
        private void btnShowHardwareProps_Click(object sender, EventArgs e) { _cameraManager.ShowProperties(0, this.Handle); }
        private void txtDataMatrix_TextChanged(object sender, EventArgs e) { UpdateCaptureButtonUI(); }

        private void UpdateCaptureButtonUI()
        {
            string currentDM = txtDataMatrix.Text.Trim();
            if (_currentPhotoIndex > 0) 
            {
                btnCapture.Enabled = true;
                int displayCamNum;
                if (_currentPhotoIndex == 1) { btnCapture.BackColor = Color.LightSkyBlue; displayCamNum = 2; }
                else if (_currentPhotoIndex == 2) { btnCapture.BackColor = Color.Orange; displayCamNum = 3; }
                else { btnCapture.BackColor = Color.Red; displayCamNum = 3; }
                btnCapture.Text = string.Format(L.T("BtnCapture_Step"), displayCamNum, _currentPhotoIndex + 1);
                return;
            }

            if (string.IsNullOrEmpty(currentDM) || currentDM.Length < 5)
            {
                btnCapture.Enabled = false; btnCapture.BackColor = SystemColors.Control;
                btnCapture.Text = L.T("BtnCapture_Wait"); txtDataMatrix.BackColor = SystemColors.Window;
            }
            else if (IsDataMatrixAlreadyProcessed(currentDM))
            {
                btnCapture.Enabled = false; btnCapture.BackColor = Color.Gray;
                btnCapture.Text = (L.Current == Language.CH) ? "重複刷入" : "Duplikat";
                txtDataMatrix.BackColor = Color.MistyRose;
            }
            else
            {
                btnCapture.Enabled = true; btnCapture.BackColor = Color.LightGreen; 
                btnCapture.Text = L.T("BtnCapture_Ready"); txtDataMatrix.BackColor = SystemColors.Window;
            }
        }

        private bool IsDataMatrixAlreadyProcessed(string dm)
        {
            if (string.IsNullOrEmpty(dm)) return false;
            string targetDir = Path.Combine(AppConfig.PhotoRootPath, DateTime.Now.ToString("yyyy-MM-dd"), dm);
            return File.Exists(Path.Combine(targetDir, "4.jpg"));
        }

        private void AddLog(string message)
        {
            if (this.IsDisposed) return;
            Utils.Logger.Info(message);
            this.BeginInvoke(new Action(() => {
                txtLog.AppendText(string.Format("[{0:HH:mm:ss}] {1}\r\n", DateTime.Now, message));
                txtLog.ScrollToCaret();
            }));
        }

        private async void btnCapture_Click(object sender, EventArgs e)
        {
            if (_isLocked) return;
            string dm = txtDataMatrix.Text.Trim();
            if (string.IsNullOrEmpty(dm)) return;

            try
            {
                btnCapture.Enabled = false;
                _currentPhotoIndex++; numPhotoCount.Value = _currentPhotoIndex;
                int actualCamIdx, displayCamNum;
                if (_currentPhotoIndex == 1) { actualCamIdx = 0; displayCamNum = 1; }
                else if (_currentPhotoIndex == 2) { actualCamIdx = 1; displayCamNum = 2; }
                else { actualCamIdx = 2; displayCamNum = 3; }

                AddLog(string.Format(L.T("LogCapturing"), _currentPhotoIndex, displayCamNum));
                await Task.Run(() => { _captureService.CaptureAndSave(dm, _currentPhotoIndex, actualCamIdx); });

                if (_currentPhotoIndex >= 4) 
                {
                    AddLog(L.T("LogCaptureDone")); _lastFinishedDmc = dm; _currentPhotoIndex = 0;
                    numPhotoCount.Value = 0; txtDataMatrix.Clear(); _cameraManager.SetPreviewIndex(0); txtDataMatrix.Focus();
                }
                else { UpdatePreviewBasedOnProgress(); }
            }
            catch (Exception ex) { AddLog("Error: " + ex.Message); _currentPhotoIndex = Math.Max(0, _currentPhotoIndex - 1); numPhotoCount.Value = _currentPhotoIndex; }
            finally { UpdateCaptureButtonUI(); }
        }

        private void UpdatePreviewBasedOnProgress()
        {
            int nextPreviewIdx, displayCamNum;
            if (_currentPhotoIndex == 0) { nextPreviewIdx = 0; displayCamNum = 1; }
            else if (_currentPhotoIndex == 1) { nextPreviewIdx = 1; displayCamNum = 2; }
            else { nextPreviewIdx = 2; displayCamNum = 3; }
            _cameraManager.SetPreviewIndex(nextPreviewIdx);
            AddLog(string.Format(L.T("LogSwitchCam"), displayCamNum));
        }

        private void btnOpenQuery_Click(object sender, EventArgs e) { new QueryForm().Show(); }
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e) { _cameraManager.StopAllCameras(); if (_scanner != null) _scanner.Stop(); }
    }
}
