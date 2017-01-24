using System;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Microsoft.Practices.Unity;

namespace API.Authorization
{
    public class ApiKeyFilter : AuthorizationFilterAttribute
    {
        private readonly IAuthManager _authManager;

        public ApiKeyFilter()
        {
            _authManager = UnityConfig.GetConfiguredContainer().Resolve<IAuthManager>();
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
                throw new UnauthorizedAccessException("Please set the 'Authorization' header to the api key of the institution!");
                //actionContext.Response = actionContext.Request.CreateResponse<object>(HttpStatusCode.Unauthorized, new { Message = "Please set the 'Authorization' header to the api key of the institution!" });
            }
            else if (!_authManager.ValidateOrganizationApiKey(apiKey.ToString()))
            {
                throw new UnauthorizedAccessException("The provided api key is not valid!");
                //actionContext.Response = actionContext.Request.CreateResponse<object>(HttpStatusCode.Unauthorized, new { Message = "The provided api key is not valid!" });
            }
        }
    }
}