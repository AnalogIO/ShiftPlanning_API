using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using ShiftPlanning.Model.Models;
using ShiftPlanning.WebApi.Exceptions;
using ShiftPlanning.WebApi.Repositories;

namespace ShiftPlanning.WebApi.Helpers.Authorization
{
    public class AuthManager : IAuthManager
    {
        private readonly IOrganizationRepository _organizationRepository;
        private readonly IEmployeeRepository _employeeRepository;

        public AuthManager(IOrganizationRepository organizationRepository, IEmployeeRepository employeeRepository)
        {
            _organizationRepository = organizationRepository;
            _employeeRepository = employeeRepository;
        }

        public bool ValidateOrganizationApiKey(string apiKey)
        {
            return _organizationRepository.HasApiKey(apiKey);
        }

        public Organization GetOrganizationByApiKey(string apiKey)
        {
            return _organizationRepository.Read(apiKey);
        }

        public Organization GetOrganizationByHeader(IHeaderDictionary headers)
        {
            headers.TryGetValue("Authorization", out var apiKey);
            if (apiKey.ToString() == null) throw new ObjectNotFoundException("Could not find a organization corresponding to the given 'Authorization' header");
            var organization = GetOrganizationByApiKey(apiKey) ?? GetEmployeeByHeader(headers).Organization;
            return organization;
        }

        public Employee GetEmployeeByHeader(IHeaderDictionary headers)
        {
            headers.TryGetValue("Authorization", out var token);
            if (token.ToString() == null) throw new ObjectNotFoundException("Could not find a manager corresponding to the given 'Authorization' header");
            return _employeeRepository.Read(token);
        }

        public bool IsManager(IHeaderDictionary headers)
        {
            headers.TryGetValue("Authorization", out var token);
            if (token.ToString() == null) throw new ObjectNotFoundException("Could not find a manager corresponding to the given 'Authorization' header");
            var employee = _employeeRepository.Read(token);
            if (employee == null) return false;
            if (employee.Role_.Any(r => r.Name == "Manager")) return true;
            return false;
        }

        public IEnumerable<Role> GetRoles(string token)
        {
            var employee = _employeeRepository.Read(token);
            return employee.Role_;
        }
    }
}