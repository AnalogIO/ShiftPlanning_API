using System.Net.Http.Headers;
using Data.Models;

namespace API.Authorization
{
    public interface IAuthManager
    {
        bool AuthenticateToken(string apikey);
        Institution GetInstitutionByApiKey(string apiKey);
        Institution GetInstitutionByHeader(HttpRequestHeaders headers);
        Manager GetManagerByHeader(HttpRequestHeaders headers);
        bool ValidateInstitutionApiKey(string apiKey);
    }
}