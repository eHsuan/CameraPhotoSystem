using System;
using System.Collections.Generic;
using System.Linq;
using LiteDB;
using CameraPhotoSystem.Model;
using CameraPhotoSystem.Config;
using CameraPhotoSystem.Utils;

namespace CameraPhotoSystem.Repository
{
    public class PhotoRepository
    {
        private readonly string _dbPath = AppConfig.DatabasePath;

        public void Insert(PhotoRecord record)
        {
            try
            {
                using (var db = new LiteDatabase(string.Format("Filename={0};Connection=shared", _dbPath)))
                {
                    var col = db.GetCollection<PhotoRecord>("photos");
                    col.Insert(record);
                    col.EnsureIndex(x => x.DataMatrix);
                }
            }
            catch (Exception ex)
            {
                Logger.Error("資料庫寫入錯誤", ex);
                throw;
            }
        }

        public List<PhotoRecord> Query(DateTime start, DateTime end, string dataMatrix)
        {
            try
            {
                using (var db = new LiteDatabase(string.Format("Filename={0};Connection=shared", _dbPath)))
                {
                    var col = db.GetCollection<PhotoRecord>("photos");
                    var query = col.Query()
                        .Where(x => x.CreateTime >= start.Date && x.CreateTime <= end.Date.AddDays(1).AddSeconds(-1));

                    if (!string.IsNullOrEmpty(dataMatrix))
                    {
                        query = query.Where(x => x.DataMatrix.Contains(dataMatrix));
                    }

                    return query.OrderByDescending(x => x.CreateTime).ToList();
                }
            }
            catch (Exception ex)
            {
                Logger.Error("資料庫查詢錯誤", ex);
                return new List<PhotoRecord>();
            }
        }
    }
}