namespace CameraPhotoSystem.UI
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.PictureBox pbCamera;
        private System.Windows.Forms.TextBox txtDataMatrix;
        private System.Windows.Forms.NumericUpDown numPhotoCount;
        private System.Windows.Forms.Button btnCapture;
        private System.Windows.Forms.Button btnOpenQuery;
        private System.Windows.Forms.Label lblDM;
        private System.Windows.Forms.Label lblCount;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.TabControl tabMain;
        private System.Windows.Forms.TabPage tpProduction;
        private System.Windows.Forms.TabPage tpEngineering;
        
        private System.Windows.Forms.GroupBox grpEngControl;
        private System.Windows.Forms.ComboBox cmbEngCameraSelect;
        private System.Windows.Forms.Label lblEngCam;
        private System.Windows.Forms.Button btnShowHardwareProps;
        private System.Windows.Forms.Label lblEngHint;
        
        private System.Windows.Forms.GroupBox grpLang;
        private System.Windows.Forms.ComboBox cmbLanguage;
        private System.Windows.Forms.Label lblLang;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.pbCamera = new System.Windows.Forms.PictureBox();
            this.tabMain = new System.Windows.Forms.TabControl();
            this.tpProduction = new System.Windows.Forms.TabPage();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.btnOpenQuery = new System.Windows.Forms.Button();
            this.btnCapture = new System.Windows.Forms.Button();
            this.numPhotoCount = new System.Windows.Forms.NumericUpDown();
            this.lblCount = new System.Windows.Forms.Label();
            this.txtDataMatrix = new System.Windows.Forms.TextBox();
            this.lblDM = new System.Windows.Forms.Label();
            this.tpEngineering = new System.Windows.Forms.TabPage();
            this.grpLang = new System.Windows.Forms.GroupBox();
            this.cmbLanguage = new System.Windows.Forms.ComboBox();
            this.lblLang = new System.Windows.Forms.Label();
            this.grpEngControl = new System.Windows.Forms.GroupBox();
            this.lblEngHint = new System.Windows.Forms.Label();
            this.btnShowHardwareProps = new System.Windows.Forms.Button();
            this.cmbEngCameraSelect = new System.Windows.Forms.ComboBox();
            this.lblEngCam = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pbCamera)).BeginInit();
            this.tabMain.SuspendLayout();
            this.tpProduction.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numPhotoCount)).BeginInit();
            this.tpEngineering.SuspendLayout();
            this.grpLang.SuspendLayout();
            this.grpEngControl.SuspendLayout();
            this.SuspendLayout();
            // 
            // pbCamera
            // 
            this.pbCamera.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pbCamera.BackColor = System.Drawing.Color.Black;
            this.pbCamera.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbCamera.Location = new System.Drawing.Point(12, 12);
            this.pbCamera.Name = "pbCamera";
            this.pbCamera.Size = new System.Drawing.Size(1525, 1546);
            this.pbCamera.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbCamera.TabIndex = 0;
            this.pbCamera.TabStop = false;
            // 
            // tabMain
            // 
            this.tabMain.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabMain.Controls.Add(this.tpProduction);
            this.tabMain.Controls.Add(this.tpEngineering);
            this.tabMain.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.tabMain.Location = new System.Drawing.Point(1545, 12);
            this.tabMain.Name = "tabMain";
            this.tabMain.SelectedIndex = 0;
            this.tabMain.Size = new System.Drawing.Size(288, 1546);
            this.tabMain.TabIndex = 1;
            this.tabMain.SelectedIndexChanged += new System.EventHandler(this.tabMain_SelectedIndexChanged);
            // 
            // tpProduction
            // 
            this.tpProduction.Controls.Add(this.txtLog);
            this.tpProduction.Controls.Add(this.btnOpenQuery);
            this.tpProduction.Controls.Add(this.btnCapture);
            this.tpProduction.Controls.Add(this.numPhotoCount);
            this.tpProduction.Controls.Add(this.lblCount);
            this.tpProduction.Controls.Add(this.txtDataMatrix);
            this.tpProduction.Controls.Add(this.lblDM);
            this.tpProduction.Location = new System.Drawing.Point(10, 71);
            this.tpProduction.Name = "tpProduction";
            this.tpProduction.Padding = new System.Windows.Forms.Padding(3);
            this.tpProduction.Size = new System.Drawing.Size(268, 1465);
            this.tpProduction.TabIndex = 0;
            this.tpProduction.Text = "Production";
            this.tpProduction.UseVisualStyleBackColor = true;
            // 
            // txtLog
            // 
            this.txtLog.BackColor = System.Drawing.Color.Black;
            this.txtLog.Font = new System.Drawing.Font("Consolas", 9F);
            this.txtLog.ForeColor = System.Drawing.Color.Lime;
            this.txtLog.Location = new System.Drawing.Point(10, 300);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLog.Size = new System.Drawing.Size(260, 410);
            this.txtLog.TabIndex = 5;
            // 
            // btnOpenQuery
            // 
            this.btnOpenQuery.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.btnOpenQuery.Location = new System.Drawing.Point(10, 716);
            this.btnOpenQuery.Name = "btnOpenQuery";
            this.btnOpenQuery.Size = new System.Drawing.Size(260, 50);
            this.btnOpenQuery.TabIndex = 6;
            this.btnOpenQuery.Text = "History";
            this.btnOpenQuery.Click += new System.EventHandler(this.btnOpenQuery_Click);
            // 
            // btnCapture
            // 
            this.btnCapture.BackColor = System.Drawing.Color.LightGreen;
            this.btnCapture.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.btnCapture.Location = new System.Drawing.Point(10, 190);
            this.btnCapture.Name = "btnCapture";
            this.btnCapture.Size = new System.Drawing.Size(260, 100);
            this.btnCapture.TabIndex = 4;
            this.btnCapture.Text = "CAPTURE";
            this.btnCapture.UseVisualStyleBackColor = false;
            this.btnCapture.Click += new System.EventHandler(this.btnCapture_Click);
            // 
            // numPhotoCount
            // 
            this.numPhotoCount.Font = new System.Drawing.Font("Arial", 22F, System.Drawing.FontStyle.Bold);
            this.numPhotoCount.Location = new System.Drawing.Point(10, 130);
            this.numPhotoCount.Name = "numPhotoCount";
            this.numPhotoCount.Size = new System.Drawing.Size(260, 92);
            this.numPhotoCount.TabIndex = 3;
            // 
            // lblCount
            // 
            this.lblCount.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblCount.Location = new System.Drawing.Point(6, 105);
            this.lblCount.Name = "lblCount";
            this.lblCount.Size = new System.Drawing.Size(260, 25);
            this.lblCount.TabIndex = 2;
            this.lblCount.Text = "Label Count";
            // 
            // txtDataMatrix
            // 
            this.txtDataMatrix.Font = new System.Drawing.Font("Arial", 22F, System.Drawing.FontStyle.Bold);
            this.txtDataMatrix.Location = new System.Drawing.Point(10, 40);
            this.txtDataMatrix.Name = "txtDataMatrix";
            this.txtDataMatrix.Size = new System.Drawing.Size(260, 92);
            this.txtDataMatrix.TabIndex = 1;
            // 
            // lblDM
            // 
            this.lblDM.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblDM.Location = new System.Drawing.Point(6, 15);
            this.lblDM.Name = "lblDM";
            this.lblDM.Size = new System.Drawing.Size(260, 25);
            this.lblDM.TabIndex = 0;
            this.lblDM.Text = "Label DM";
            // 
            // tpEngineering
            // 
            this.tpEngineering.Controls.Add(this.grpLang);
            this.tpEngineering.Controls.Add(this.grpEngControl);
            this.tpEngineering.Location = new System.Drawing.Point(10, 71);
            this.tpEngineering.Name = "tpEngineering";
            this.tpEngineering.Padding = new System.Windows.Forms.Padding(3);
            this.tpEngineering.Size = new System.Drawing.Size(268, 729);
            this.tpEngineering.TabIndex = 1;
            this.tpEngineering.Text = "Technic";
            this.tpEngineering.UseVisualStyleBackColor = true;
            // 
            // grpLang
            // 
            this.grpLang.Controls.Add(this.cmbLanguage);
            this.grpLang.Controls.Add(this.lblLang);
            this.grpLang.Location = new System.Drawing.Point(6, 371);
            this.grpLang.Name = "grpLang";
            this.grpLang.Size = new System.Drawing.Size(268, 120);
            this.grpLang.TabIndex = 1;
            this.grpLang.TabStop = false;
            this.grpLang.Text = "Language";
            // 
            // cmbLanguage
            // 
            this.cmbLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLanguage.FormattingEnabled = true;
            this.cmbLanguage.Items.AddRange(new object[] {
            "繁體中文 (CH)",
            "Deutsch (DE)"});
            this.cmbLanguage.Location = new System.Drawing.Point(10, 65);
            this.cmbLanguage.Name = "cmbLanguage";
            this.cmbLanguage.Size = new System.Drawing.Size(240, 62);
            this.cmbLanguage.TabIndex = 1;
            this.cmbLanguage.SelectedIndexChanged += new System.EventHandler(this.cmbLanguage_SelectedIndexChanged);
            // 
            // lblLang
            // 
            this.lblLang.Location = new System.Drawing.Point(10, 30);
            this.lblLang.Name = "lblLang";
            this.lblLang.Size = new System.Drawing.Size(240, 25);
            this.lblLang.TabIndex = 0;
            this.lblLang.Text = "Select Language:";
            // 
            // grpEngControl
            // 
            this.grpEngControl.Controls.Add(this.lblEngHint);
            this.grpEngControl.Controls.Add(this.btnShowHardwareProps);
            this.grpEngControl.Controls.Add(this.cmbEngCameraSelect);
            this.grpEngControl.Controls.Add(this.lblEngCam);
            this.grpEngControl.Location = new System.Drawing.Point(6, 15);
            this.grpEngControl.Name = "grpEngControl";
            this.grpEngControl.Size = new System.Drawing.Size(268, 359);
            this.grpEngControl.TabIndex = 0;
            this.grpEngControl.TabStop = false;
            this.grpEngControl.Text = "Hardware";
            // 
            // lblEngHint
            // 
            this.lblEngHint.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblEngHint.ForeColor = System.Drawing.Color.DimGray;
            this.lblEngHint.Location = new System.Drawing.Point(10, 220);
            this.lblEngHint.Name = "lblEngHint";
            this.lblEngHint.Size = new System.Drawing.Size(240, 60);
            this.lblEngHint.TabIndex = 3;
            this.lblEngHint.Text = "Hint";
            // 
            // btnShowHardwareProps
            // 
            this.btnShowHardwareProps.BackColor = System.Drawing.Color.LightSkyBlue;
            this.btnShowHardwareProps.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnShowHardwareProps.Location = new System.Drawing.Point(10, 130);
            this.btnShowHardwareProps.Name = "btnShowHardwareProps";
            this.btnShowHardwareProps.Size = new System.Drawing.Size(240, 80);
            this.btnShowHardwareProps.TabIndex = 2;
            this.btnShowHardwareProps.Text = "Open Setup";
            this.btnShowHardwareProps.UseVisualStyleBackColor = false;
            this.btnShowHardwareProps.Click += new System.EventHandler(this.btnShowHardwareProps_Click);
            // 
            // cmbEngCameraSelect
            // 
            this.cmbEngCameraSelect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbEngCameraSelect.FormattingEnabled = true;
            this.cmbEngCameraSelect.Items.AddRange(new object[] {
            "Kamera 1",
            "Kamera 2",
            "Kamera 3"});
            this.cmbEngCameraSelect.Location = new System.Drawing.Point(10, 60);
            this.cmbEngCameraSelect.Name = "cmbEngCameraSelect";
            this.cmbEngCameraSelect.Size = new System.Drawing.Size(240, 62);
            this.cmbEngCameraSelect.TabIndex = 1;
            this.cmbEngCameraSelect.SelectedIndexChanged += new System.EventHandler(this.cmbEngCameraSelect_SelectedIndexChanged);
            // 
            // lblEngCam
            // 
            this.lblEngCam.Location = new System.Drawing.Point(6, 35);
            this.lblEngCam.Name = "lblEngCam";
            this.lblEngCam.Size = new System.Drawing.Size(200, 25);
            this.lblEngCam.TabIndex = 0;
            this.lblEngCam.Text = "Select Camera";
            // 
            // MainForm
            // 
            this.ClientSize = new System.Drawing.Size(1845, 1586);
            this.Controls.Add(this.tabMain);
            this.Controls.Add(this.pbCamera);
            this.Name = "MainForm";
            this.Text = "Camera Photo System";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbCamera)).EndInit();
            this.tabMain.ResumeLayout(false);
            this.tpProduction.ResumeLayout(false);
            this.tpProduction.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numPhotoCount)).EndInit();
            this.tpEngineering.ResumeLayout(false);
            this.grpLang.ResumeLayout(false);
            this.grpEngControl.ResumeLayout(false);
            this.ResumeLayout(false);

        }
    }
}