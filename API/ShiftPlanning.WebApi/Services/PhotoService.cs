using ShiftPlanning.Model.Models;
using ShiftPlanning.WebApi.Repositories;

namespace ShiftPlanning.WebApi.Services
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

        public Photo CreatePhoto(Photo photo, Employee employee)
        {
            if (photo.Organization != employee.Organization)
            {
                photo.Organization = employee.Organization;
            }

            return _photoRepository.Create(photo);
        }
    }
}
