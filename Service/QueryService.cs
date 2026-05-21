using System;
using System.Collections.Generic;
using CameraPhotoSystem.Model;
using CameraPhotoSystem.Repository;

namespace CameraPhotoSystem.Service
{
    public class QueryService
    {
        private readonly PhotoRepository _repository;

        public QueryService()
        {
            _repository = new PhotoRepository();
        }

        public List<PhotoRecord> SearchPhotos(DateTime start, DateTime end, string dataMatrix)
        {
            return _repository.Query(start, end, dataMatrix);
        }
    }
}