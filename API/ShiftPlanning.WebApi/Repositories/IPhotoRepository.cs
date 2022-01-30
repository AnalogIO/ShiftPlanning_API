using ShiftPlanning.Model.Models;

namespace ShiftPlanning.WebApi.Repositories
{
    public interface IPhotoRepository
    {
        Photo Read(int photoId, string organizationShortKey);
        Photo Read(int photoId, int organizationId);

        Photo Create(Photo photo);
    }
}
