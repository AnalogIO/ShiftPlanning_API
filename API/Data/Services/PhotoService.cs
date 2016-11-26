using System;
using Data.Models;
using Data.Repositories;

namespace Data.Services
{
    public class PhotoService : IPhotoService
    {
        private readonly IPhotoRepository _photoRepository;

        public PhotoService(IPhotoRepository photoRepository)
        {
            _photoRepository = photoRepository;
        }

        public Photo Read(int photoId, int organizationId)
        {
            return _photoRepository.Read(photoId, organizationId);
        }

        public Photo Read(int photoId, string organizationShortKey)
        {
            return _photoRepository.Read(photoId, organizationShortKey);
        }
    }
}
