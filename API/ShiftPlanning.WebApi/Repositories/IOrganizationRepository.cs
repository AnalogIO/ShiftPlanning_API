using ShiftPlanning.Model.Models;

namespace ShiftPlanning.WebApi.Repositories
{
    public interface IOrganizationRepository
    {
        bool Exists(string shortKey);
        bool Exists(int id);
        bool HasApiKey(string apiKey);
        Organization Read(int id);
        Organization Read(string apiKey);
        Organization ReadByShortKey(string shortKey);
    }
}
