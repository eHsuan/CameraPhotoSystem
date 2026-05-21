using System;
using LiteDB;

namespace CameraPhotoSystem.Model
{
    public class PhotoRecord
    {
        [LiteDB.BsonId]
        [System.ComponentModel.Browsable(false)] // 隱藏此欄位，不顯示於 UI
        public Guid Id { get; set; }
        
        [System.ComponentModel.DisplayName("DataMatrix 條碼")]
        public string DataMatrix { get; set; }

        [System.ComponentModel.DisplayName("存檔路徑")]
        public string PhotoPath { get; set; }

        [System.ComponentModel.DisplayName("照片序號")]
        public int PhotoIndex { get; set; }

        [System.ComponentModel.DisplayName("建立時間")]
        public DateTime CreateTime { get; set; }
    }
}