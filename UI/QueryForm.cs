using System;
using System.Windows.Forms;
using CameraPhotoSystem.Service;
using CameraPhotoSystem.Model;
using System.Drawing;

namespace CameraPhotoSystem.UI
{
    public partial class QueryForm : Form
    {
        private readonly QueryService _queryService;
        private float _zoomLevel = 1.0f;
        private Point _lastMousePos;

        public QueryForm()
        {
            InitializeComponent();
            this.Font = new Font("Segoe UI", 9F);
            _queryService = new QueryService();
            dtpFrom.Value = DateTime.Now.Date;
            dtpTo.Value = DateTime.Now.Date;

            // 註冊滾輪事件
            this.pbPreview.MouseWheel += PbPreview_MouseWheel;
            this.pbPreview.MouseDown += PbPreview_MouseDown;
            this.pbPreview.MouseMove += PbPreview_MouseMove;
        }

        private void PbPreview_MouseWheel(object sender, MouseEventArgs e)
        {
            if (pbPreview.Image == null) return;

            // 調整縮放倍率 (每次 10%)
            if (e.Delta > 0) _zoomLevel += 0.1f;
            else _zoomLevel -= 0.1f;

            if (_zoomLevel < 0.1f) _zoomLevel = 0.1f;
            if (_zoomLevel > 10.0f) _zoomLevel = 10.0f;

            ApplyZoom();
        }

        private void ApplyZoom()
        {
            if (pbPreview.Image == null) return;

            // 計算縮放後的大小 (維持比例)
            int newWidth = (int)(pnlPreviewContainer.Width * _zoomLevel);
            int newHeight = (int)(pnlPreviewContainer.Height * _zoomLevel);

            pbPreview.Size = new Size(newWidth, newHeight);
            
            // 如果縮放小於容器，置中顯示
            if (_zoomLevel <= 1.0f)
            {
                pbPreview.Location = new Point(
                    (pnlPreviewContainer.Width - pbPreview.Width) / 2,
                    (pnlPreviewContainer.Height - pbPreview.Height) / 2
                );
            }
            else
            {
                pbPreview.Location = new Point(0, 0);
            }
        }

        private void PbPreview_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _lastMousePos = e.Location;
            }
        }

        private void PbPreview_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && _zoomLevel > 1.0f)
            {
                // 計算拖動位移並調整 Panel 的捲軸位置
                int deltaX = _lastMousePos.X - e.X;
                int deltaY = _lastMousePos.Y - e.Y;

                pnlPreviewContainer.AutoScrollPosition = new Point(
                    -pnlPreviewContainer.AutoScrollPosition.X + deltaX,
                    -pnlPreviewContainer.AutoScrollPosition.Y + deltaY
                );
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            var results = _queryService.SearchPhotos(dtpFrom.Value, dtpTo.Value, txtDataMatrix.Text);
            dgvResults.DataSource = results;
        }

        private void dgvResults_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvResults.CurrentRow != null)
            {
                PhotoRecord record = dgvResults.CurrentRow.DataBoundItem as PhotoRecord;
                if (record != null)
                {
                    try
                    {
                        if (System.IO.File.Exists(record.PhotoPath))
                        {
                            using (var fs = new System.IO.FileStream(record.PhotoPath, System.IO.FileMode.Open, System.IO.FileAccess.Read))
                            {
                                var ms = new System.IO.MemoryStream();
                                fs.CopyTo(ms);
                                ms.Position = 0;
                                
                                var oldImage = pbPreview.Image;
                                pbPreview.Image = Image.FromStream(ms);
                                
                                if (oldImage != null)
                                {
                                    oldImage.Dispose();
                                }
                                
                                // 重置縮放
                                _zoomLevel = 1.0f;
                                ApplyZoom();
                            }
                        }
                        else
                        {
                            pbPreview.Image = null;
                        }
                    }
                    catch
                    {
                        pbPreview.Image = null;
                    }
                }
            }
        }
    }
}