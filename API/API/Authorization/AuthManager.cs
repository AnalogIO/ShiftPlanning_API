using System.Configuration;
using Data.Repositories;
using Data.Models;
using System.Net.Http.Headers;

namespace API.Authorization
{
    public class AuthManager : IAuthManager
    {
        private readonly IInstitutionRepository _institutionRepository;
        private readonly IManagerRepository _managerRepository;

        public AuthManager(IInstitutionRepository institutionRepository, IManagerRepository managerRepository)
        {
            _institutionRepository = institutionRepository;
            _managerRepository = managerRepository;
        }
        
        // old implementation not used anymore - now authorizing manager tokens instead
        public bool AuthenticateToken(string apikey)
        {
            return apikey.Equals(ConfigurationManager.AppSettings["ApiKey"]);
        }

        public bool ValidateInstitutionApiKey(string apiKey)
        {
            return _institutionRepository.HasApiKey(apiKey);
        }

        public Institution GetInstitutionByHeader(HttpRequestHeaders headers)
        {
            var apiKey = headers.Authorization.ToString();
            if (apiKey == null) return null;
            return _institutionRepository.Read(apiKey);
        }

        public Manager GetManagerByHeader(HttpRequestHeaders headers)
        {
            var token = headers.Authorization.ToString();
            if (token == null) return null;
            return _managerRepository.Read(token);
        }
    }
}