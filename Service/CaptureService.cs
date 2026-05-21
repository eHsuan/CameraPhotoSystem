using System;
using System.Drawing;
using System.IO;
using System.Linq;
using CameraPhotoSystem.Camera;
using CameraPhotoSystem.Config;
using CameraPhotoSystem.Model;
using CameraPhotoSystem.Repository;
using CameraPhotoSystem.Utils;

namespace CameraPhotoSystem.Service
{
    public class CaptureService
    {
        private readonly CameraManager _cameraManager;
        private readonly PhotoRepository _repository;

        public CaptureService(CameraManager cameraManager)
        {
            _cameraManager = cameraManager;
            _repository = new PhotoRepository();
        }

        // 恢復為手動傳參存檔模式
        public void CaptureAndSave(string dataMatrix, int photoIndex, int cameraIndex)
        {
            try
            {
                // 從指定索引的相機抓取最新影格
                Bitmap bmp = _cameraManager.CaptureImageFromIndex(cameraIndex, 2000);
                if (bmp == null) throw new Exception(string.Format("無法從相機 {0} 獲取影像", cameraIndex + 1));

                // 建立新的結構路徑：PhotoRoot\yyyy-MM-dd\DataMatrix\
                string dateFolder = DateTime.Now.ToString("yyyy-MM-dd");
                string targetDir = Path.Combine(AppConfig.PhotoRootPath, dateFolder, dataMatrix);
                
                // 自動建立層級目錄 (包含 DataMatrix 資料夾)
                if (!Directory.Exists(targetDir)) Directory.CreateDirectory(targetDir);

                // 檔名改為純序號 (或保留 DM 前綴，這裡改為 1.jpg)
                string fileName = string.Format("{0}.jpg", photoIndex);
                string fullPath = Path.Combine(targetDir, fileName);

                // 高品質存檔 (品質 100)
                var encoderParams = new System.Drawing.Imaging.EncoderParameters(1);
                encoderParams.Param[0] = new System.Drawing.Imaging.EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 100L);
                var jpegEncoder = GetEncoder(System.Drawing.Imaging.ImageFormat.Jpeg);
                
                bmp.Save(fullPath, jpegEncoder, encoderParams);
                bmp.Dispose();

                var record = new PhotoRecord
                {
                    Id = Guid.NewGuid(),
                    DataMatrix = dataMatrix,
                    PhotoPath = fullPath,
                    PhotoIndex = photoIndex,
                    CreateTime = DateTime.Now
                };

                _repository.Insert(record);
                Logger.Info(string.Format("拍照存檔成功 (新結構): {0}", fullPath));
            }
            catch (Exception ex)
            {
                Logger.Error("拍照或存檔流程失敗", ex);
                throw;
            }
        }

        private System.Drawing.Imaging.ImageCodecInfo GetEncoder(System.Drawing.Imaging.ImageFormat format)
        {
            var codecs = System.Drawing.Imaging.ImageCodecInfo.GetImageDecoders();
            foreach (var codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }
    }
}