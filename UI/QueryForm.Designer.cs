namespace CameraPhotoSystem.UI
{
    partial class QueryForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.DateTimePicker dtpTo;
        private System.Windows.Forms.TextBox txtDataMatrix;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.DataGridView dgvResults;
        private System.Windows.Forms.PictureBox pbPreview;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private System.Windows.Forms.Panel pnlPreviewContainer;

        private void InitializeComponent()
        {
            this.dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.dtpTo = new System.Windows.Forms.DateTimePicker();
            this.txtDataMatrix = new System.Windows.Forms.TextBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.dgvResults = new System.Windows.Forms.DataGridView();
            this.pnlPreviewContainer = new System.Windows.Forms.Panel();
            this.pbPreview = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvResults)).BeginInit();
            this.pnlPreviewContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbPreview)).BeginInit();
            this.SuspendLayout();

            this.dtpFrom.Location = new System.Drawing.Point(12, 12);
            this.dtpFrom.Size = new System.Drawing.Size(150, 25);
            
            this.dtpTo.Location = new System.Drawing.Point(170, 12);
            this.dtpTo.Size = new System.Drawing.Size(150, 25);

            this.txtDataMatrix.Location = new System.Drawing.Point(330, 12);
            this.txtDataMatrix.Size = new System.Drawing.Size(200, 25);

            this.btnSearch.Location = new System.Drawing.Point(540, 10);
            this.btnSearch.Size = new System.Drawing.Size(100, 30);
            this.btnSearch.Text = "執行查詢";
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);

            // 清單區域
            this.dgvResults.Location = new System.Drawing.Point(12, 50);
            this.dgvResults.Size = new System.Drawing.Size(500, 750);
            this.dgvResults.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
            this.dgvResults.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvResults.SelectionChanged += new System.EventHandler(this.dgvResults_SelectionChanged);

            // 預覽容器 (Panel)
            this.pnlPreviewContainer.Location = new System.Drawing.Point(520, 50);
            this.pnlPreviewContainer.Size = new System.Drawing.Size(750, 750);
            this.pnlPreviewContainer.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right);
            this.pnlPreviewContainer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlPreviewContainer.AutoScroll = true; // 開啟自動捲軸

            // 圖片方框 (PictureBox)
            this.pbPreview.Location = new System.Drawing.Point(0, 0);
            this.pbPreview.Size = new System.Drawing.Size(740, 740);
            this.pbPreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbPreview.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.pbPreview.Cursor = System.Windows.Forms.Cursors.Hand; // 顯示手型 cursor

            this.pnlPreviewContainer.Controls.Add(this.pbPreview);

            this.ClientSize = new System.Drawing.Size(1284, 821);
            this.Controls.Add(this.pnlPreviewContainer);
            this.Controls.Add(this.dgvResults);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.txtDataMatrix);
            this.Controls.Add(this.dtpTo);
            this.Controls.Add(this.dtpFrom);
            this.Text = "歷史照片查詢 (滾輪放大縮小 / 拖動圖片)";
            ((System.ComponentModel.ISupportInitialize)(this.dgvResults)).EndInit();
            this.pnlPreviewContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbPreview)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}