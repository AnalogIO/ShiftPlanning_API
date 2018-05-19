using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using Data.Repositories;
using Data.Models;
using System.Net.Http.Headers;

namespace API.Authorization
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

        public Organization GetOrganizationByHeader(HttpRequestHeaders headers)
        {
            var apiKey = headers.Authorization.ToString();
            if (apiKey == null) throw new ObjectNotFoundException("Could not find a organization corresponding to the given 'Authorization' header");
            var organization = GetOrganizationByApiKey(apiKey) ?? GetEmployeeByHeader(headers).Organization;
            return organization;
        }

        public Employee GetEmployeeByHeader(HttpRequestHeaders headers)
        {
            var token = headers.Authorization.ToString();
            if (token == null) throw new ObjectNotFoundException("Could not find a manager corresponding to the given 'Authorization' header");
            return _employeeRepository.Read(token);
        }

        public bool IsManager(HttpRequestHeaders headers)
        {
            var token = headers.Authorization.ToString();
            if (token == null) throw new ObjectNotFoundException("Could not find a manager corresponding to the given 'Authorization' header");
            var employee = _employeeRepository.Read(token);
            if (employee == null) return false;
            if (employee.Roles.Any(r => r.Name == "Manager")) return true;
            return false;
        }

        public IEnumerable<Role> GetRoles(string token)
        {
            var employee = _employeeRepository.Read(token);
            return employee.Roles;
        }
    }
}