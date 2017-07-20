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
        private readonly IManagerRepository _managerRepository;
        private readonly IEmployeeRepository _employeeRepository;

        public AuthManager(IOrganizationRepository organizationRepository, IManagerRepository managerRepository, IEmployeeRepository employeeRepository)
        {
            _organizationRepository = organizationRepository;
            _managerRepository = managerRepository;
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
            return GetOrganizationByApiKey(apiKey);
        }

        public Manager GetManagerByHeader(HttpRequestHeaders headers)
        {
            var token = headers.Authorization.ToString();
            if (token == null) throw new ObjectNotFoundException("Could not find a manager corresponding to the given 'Authorization' header");
            return _managerRepository.Read(token);
        }

        public bool ValidateRole(string token, string[] roles)
        {
            var employee = _employeeRepository.Read(token);
            return roles.Any(role => employee.Roles.Any(r => r.Name == role));
        }
    }
}