namespace CameraPhotoSystem.UI
{
    partial class SettingForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblLineName;
        private System.Windows.Forms.TextBox txtLineName;
        private System.Windows.Forms.Label lblPhotoRoot;
        private System.Windows.Forms.TextBox txtPhotoRoot;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.Label lblMaxCount;
        private System.Windows.Forms.NumericUpDown numMaxCount;
        private System.Windows.Forms.Label lblDesiredWidth;
        private System.Windows.Forms.ComboBox cmbDesiredWidth;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblLineName = new System.Windows.Forms.Label();
            this.txtLineName = new System.Windows.Forms.TextBox();
            this.lblPhotoRoot = new System.Windows.Forms.Label();
            this.txtPhotoRoot = new System.Windows.Forms.TextBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.lblMaxCount = new System.Windows.Forms.Label();
            this.numMaxCount = new System.Windows.Forms.NumericUpDown();
            this.lblDesiredWidth = new System.Windows.Forms.Label();
            this.cmbDesiredWidth = new System.Windows.Forms.ComboBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numMaxCount)).BeginInit();
            this.SuspendLayout();
            // 
            // lblLineName
            // 
            this.lblLineName.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblLineName.Location = new System.Drawing.Point(20, 20);
            this.lblLineName.Name = "lblLineName";
            this.lblLineName.Size = new System.Drawing.Size(420, 22);
            this.lblLineName.TabIndex = 0;
            this.lblLineName.Text = "產線名稱 (Line Name):";
            // 
            // txtLineName
            // 
            this.txtLineName.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.txtLineName.Location = new System.Drawing.Point(20, 45);
            this.txtLineName.Name = "txtLineName";
            this.txtLineName.Size = new System.Drawing.Size(420, 32);
            this.txtLineName.TabIndex = 1;
            // 
            // lblPhotoRoot
            // 
            this.lblPhotoRoot.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblPhotoRoot.Location = new System.Drawing.Point(20, 90);
            this.lblPhotoRoot.Name = "lblPhotoRoot";
            this.lblPhotoRoot.Size = new System.Drawing.Size(420, 22);
            this.lblPhotoRoot.TabIndex = 2;
            this.lblPhotoRoot.Text = "照片存放目錄 (Photo Root):";
            // 
            // txtPhotoRoot
            // 
            this.txtPhotoRoot.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtPhotoRoot.Location = new System.Drawing.Point(20, 115);
            this.txtPhotoRoot.Name = "txtPhotoRoot";
            this.txtPhotoRoot.Size = new System.Drawing.Size(325, 30);
            this.txtPhotoRoot.TabIndex = 3;
            // 
            // btnBrowse
            // 
            this.btnBrowse.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnBrowse.Location = new System.Drawing.Point(355, 114);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(85, 32);
            this.btnBrowse.TabIndex = 4;
            this.btnBrowse.Text = "瀏覽...";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // lblMaxCount
            // 
            this.lblMaxCount.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblMaxCount.Location = new System.Drawing.Point(20, 160);
            this.lblMaxCount.Name = "lblMaxCount";
            this.lblMaxCount.Size = new System.Drawing.Size(420, 22);
            this.lblMaxCount.TabIndex = 5;
            this.lblMaxCount.Text = "最大拍照張數 (Max Photos):";
            // 
            // numMaxCount
            // 
            this.numMaxCount.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.numMaxCount.Location = new System.Drawing.Point(20, 185);
            this.numMaxCount.Maximum = new decimal(new int[] { 20, 0, 0, 0 });
            this.numMaxCount.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            this.numMaxCount.Name = "numMaxCount";
            this.numMaxCount.Size = new System.Drawing.Size(420, 32);
            this.numMaxCount.TabIndex = 6;
            this.numMaxCount.Value = new decimal(new int[] { 5, 0, 0, 0 });
            // 
            // lblDesiredWidth
            // 
            this.lblDesiredWidth.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblDesiredWidth.Location = new System.Drawing.Point(20, 230);
            this.lblDesiredWidth.Name = "lblDesiredWidth";
            this.lblDesiredWidth.Size = new System.Drawing.Size(420, 22);
            this.lblDesiredWidth.TabIndex = 7;
            this.lblDesiredWidth.Text = "相機目標寬度解析度:";
            // 
            // cmbDesiredWidth
            // 
            this.cmbDesiredWidth.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.cmbDesiredWidth.FormattingEnabled = true;
            this.cmbDesiredWidth.Items.AddRange(new object[] {
            "3840 (4K UHD)",
            "1920 (1080p FHD)",
            "1280 (720p HD)"});
            this.cmbDesiredWidth.Location = new System.Drawing.Point(20, 255);
            this.cmbDesiredWidth.Name = "cmbDesiredWidth";
            this.cmbDesiredWidth.Size = new System.Drawing.Size(420, 33);
            this.cmbDesiredWidth.TabIndex = 8;
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.LightGreen;
            this.btnSave.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Bold);
            this.btnSave.Location = new System.Drawing.Point(215, 315);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(110, 42);
            this.btnSave.TabIndex = 9;
            this.btnSave.Text = "儲存設定";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.btnCancel.Location = new System.Drawing.Point(335, 315);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(105, 42);
            this.btnCancel.TabIndex = 10;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // SettingForm
            // 
            this.AcceptButton = this.btnSave;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(464, 375);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.cmbDesiredWidth);
            this.Controls.Add(this.lblDesiredWidth);
            this.Controls.Add(this.numMaxCount);
            this.Controls.Add(this.lblMaxCount);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.txtPhotoRoot);
            this.Controls.Add(this.lblPhotoRoot);
            this.Controls.Add(this.txtLineName);
            this.Controls.Add(this.lblLineName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "系統參數設定";
            this.Load += new System.EventHandler(this.SettingForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numMaxCount)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
