using System;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Data.Token;

namespace API.Authorization
{
    public class AdminFilterAttribute : AuthorizationFilterAttribute
    {
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
                    actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "The provided token is not valid!");
                }
            }
        }

    }
}