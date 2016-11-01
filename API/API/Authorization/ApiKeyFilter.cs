using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Microsoft.Practices.Unity;

namespace API.Authorization
{
    public class ApiKeyFilter : AuthorizationFilterAttribute
    {
        private readonly AuthManager _authManager;

        public ApiKeyFilter()
        {
            _authManager = UnityConfig.GetConfiguredContainer().Resolve<AuthManager>();
        }

        /// <summary>
        /// OnAuthorization is called whenever a method has the data annotation "[ApiKeyFilter]".
        /// Checks if the user is authorized.
        /// </summary>
        /// <param name="actionContext"></param>
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            var apiKey = actionContext.Request.Headers.Authorization;
            if (apiKey == null)
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
            }
            else if (!_authManager.ValidateInstitutionApiKey(apiKey.ToString()))
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
            }
        }
    }
}