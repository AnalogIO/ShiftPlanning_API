using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using ShiftPlanning.Model.Models;

namespace ShiftPlanning.WebApi.Helpers.Authorization
{
    public interface IAuthManager
    {
        Organization GetOrganizationByApiKey(string apiKey);
        Organization GetOrganizationByHeader(IHeaderDictionary headers);
        Employee GetEmployeeByHeader(IHeaderDictionary headers);
        bool ValidateOrganizationApiKey(string apiKey);
        IEnumerable<Role> GetRoles(string token);
        bool IsManager(IHeaderDictionary headers);
    }
}