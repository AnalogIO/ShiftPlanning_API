using ShiftPlanning.Model.Models;

namespace ShiftPlanning.WebApi.Services
{
    public interface IPhotoService
    {
        Photo Read(int photoId, string organizationShortKey);
        Photo Read(int photoId, int organizationId);
        Photo CreatePhoto(Photo photo, Employee employee);
    }
}
