using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace API.Logic
{
    public class AuthManager
    {
        // old implementation not used anymore - now authorizing manager tokens instead
        public static bool AuthenticateToken(string apikey)
        {
            return apikey.Equals(ConfigurationManager.AppSettings["ApiKey"]);
        }

    }
}