using System.Configuration;
using Data.Npgsql.Repositories;
using Data.Repositories;

namespace API.Logic
{
    public class AuthManager
    {
        private readonly IInstitutionRepository _institutionRepository;

        public AuthManager(IInstitutionRepository institutionRepository)
        {
            _institutionRepository = institutionRepository;
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
    }
}