using ShiftPlanning.Model.Models;

namespace ShiftPlanning.WebApi.Helpers.Mappers
{
    public interface IPhotoMapper
    {
        Photo ParseBase64Photo(string base64EncodedPhoto, Organization organization);
    }
}