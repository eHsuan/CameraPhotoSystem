using System;
using System.IO;
using System.Windows.Forms;
using CameraPhotoSystem.Config;

namespace CameraPhotoSystem.UI
{
    public partial class SettingForm : Form
    {
        public SettingForm()
        {
            InitializeComponent();
        }

        private void SettingForm_Load(object sender, EventArgs e)
        {
            ApplyLanguage();
            LoadCurrentSettings();
        }

        private void ApplyLanguage()
        {
            this.Text = L.T("SettingTitle");
            lblLineName.Text = L.T("LblLineName");
            lblPhotoRoot.Text = L.T("LblPhotoRoot");
            lblUploadPath.Text = L.T("LblUploadPath");
            lblMaxCount.Text = L.T("LblMaxCount");
            lblDesiredWidth.Text = L.T("LblDesiredWidth");
            btnBrowse.Text = L.T("BtnBrowse");
            btnBrowseUpload.Text = L.T("BtnBrowse");
            btnSave.Text = L.T("BtnSave");
            btnCancel.Text = L.T("BtnCancel");
        }

        private void LoadCurrentSettings()
        {
            txtLineName.Text = AppConfig.LineName;
            txtPhotoRoot.Text = AppConfig.PhotoRootPath;
            txtUploadPath.Text = AppConfig.UploadPath;
            numMaxCount.Value = Math.Max(numMaxCount.Minimum, Math.Min(numMaxCount.Maximum, AppConfig.MaxPhotoCount));

            int currentWidth = AppConfig.DesiredWidth;
            bool matched = false;
            for (int i = 0; i < cmbDesiredWidth.Items.Count; i++)
            {
                if (cmbDesiredWidth.Items[i].ToString().StartsWith(currentWidth.ToString()))
                {
                    cmbDesiredWidth.SelectedIndex = i;
                    matched = true;
                    break;
                }
            }
            if (!matched)
            {
                cmbDesiredWidth.Text = currentWidth.ToString();
            }
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                fbd.Description = (L.Current == Language.CH) ? "請選擇照片儲存根目錄" : "Bitte Zielordner für Fotos wählen";
                if (Directory.Exists(txtPhotoRoot.Text.Trim()))
                {
                    fbd.SelectedPath = txtPhotoRoot.Text.Trim();
                }

                if (fbd.ShowDialog(this) == DialogResult.OK)
                {
                    txtPhotoRoot.Text = fbd.SelectedPath;
                }
            }
        }

        private void btnBrowseUpload_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                fbd.Description = (L.Current == Language.CH) ? "請選擇 CSV 上傳目標目錄" : "Bitte Zielordner für CSV-Upload wählen";
                if (Directory.Exists(txtUploadPath.Text.Trim()))
                {
                    fbd.SelectedPath = txtUploadPath.Text.Trim();
                }

                if (fbd.ShowDialog(this) == DialogResult.OK)
                {
                    txtUploadPath.Text = fbd.SelectedPath;
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string lineName = txtLineName.Text.Trim();
            if (string.IsNullOrEmpty(lineName))
            {
                MessageBox.Show((L.Current == Language.CH) ? "產線名稱不能為空！" : "Linienname darf nicht leer sein!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtLineName.Focus();
                return;
            }

            string photoPath = txtPhotoRoot.Text.Trim();
            if (string.IsNullOrEmpty(photoPath))
            {
                MessageBox.Show((L.Current == Language.CH) ? "照片儲存目錄不能為空！" : "Fotopfad darf nicht leer sein!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPhotoRoot.Focus();
                return;
            }

            string uploadPath = txtUploadPath.Text.Trim();
            if (string.IsNullOrEmpty(uploadPath))
            {
                MessageBox.Show((L.Current == Language.CH) ? "上傳目錄不能為空！" : "Upload-Pfad darf nicht leer sein!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUploadPath.Focus();
                return;
            }

            try
            {
                if (!Directory.Exists(photoPath))
                {
                    Directory.CreateDirectory(photoPath);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format((L.Current == Language.CH) ? "無法建立照片儲存目錄: {0}" : "Fotopfad kann nicht erstellt werden: {0}", ex.Message), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int maxCount = (int)numMaxCount.Value;

            int desiredWidth = 3840;
            string selectedWidthStr = cmbDesiredWidth.Text.Trim();
            if (selectedWidthStr.Contains(" "))
            {
                selectedWidthStr = selectedWidthStr.Split(' ')[0];
            }
            if (!int.TryParse(selectedWidthStr, out desiredWidth) || desiredWidth <= 0)
            {
                desiredWidth = 3840;
            }

            AppConfig.UpdateSettings(lineName, photoPath, maxCount, desiredWidth, uploadPath);

            MessageBox.Show(L.T("MsgSaveSuccess"), "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
