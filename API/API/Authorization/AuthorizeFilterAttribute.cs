using System;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Data.Token;
using Microsoft.Practices.Unity;

namespace API.Authorization
{
    public class AuthorizeFilterAttribute : AuthorizationFilterAttribute
    {
        string _roles;
        private readonly IAuthManager _authManager;

        public AuthorizeFilterAttribute(string roles)
        {
            _roles = roles;
            _authManager = UnityConfig.GetConfiguredContainer().Resolve<IAuthManager>();
        }

        /// <summary>
        /// OnAuthorization is called whenever a method has the data annotation "[AdminFilter]".
        /// Checks if the manager is authorized.
        /// </summary>
        /// <param name="actionContext"></param>
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            var token = actionContext.Request.Headers.Authorization;
            if (token == null)
            {
                actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Please set the 'Authorization' header to the token granted on login!");
            }
            else
            {
                if (!TokenManager.ValidateLoginToken(token.ToString()))
                {
                    actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.Unauthorized,
                        "The provided token is not valid!");
                }
                else
                {
                    if (!_authManager.ValidateRole(token.ToString(), _roles.Split(',')))
                    {
                        actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.Unauthorized,
                        "The user does not have the required privileges to perform that action!");
                    }
                }
            }
        }

    }
}