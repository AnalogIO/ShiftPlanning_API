using System.Collections.Generic;
using System.Net.Http.Headers;
using Data.Models;

namespace API.Authorization
{
    public interface IAuthManager
    {
        Organization GetOrganizationByApiKey(string apiKey);
        Organization GetOrganizationByHeader(HttpRequestHeaders headers);
        Employee GetEmployeeByHeader(HttpRequestHeaders headers);
        bool ValidateOrganizationApiKey(string apiKey);
        IEnumerable<Role> GetRoles(string token);
    }
}