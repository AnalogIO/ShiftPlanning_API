using Data.Models;

namespace Data.Services
{
    public interface IPhotoService
    {
        Photo Read(int photoId, string organizationShortKey);
        Photo Read(int photoId, int organizationId);
    }
}
