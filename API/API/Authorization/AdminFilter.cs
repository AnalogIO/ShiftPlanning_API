using System;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Data.Token;

namespace API.Authorization
{
    public class AdminFilter : AuthorizationFilterAttribute
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
                throw new UnauthorizedAccessException("Please set the 'Authorization' header to the token granted on login!");
                //actionContext.Response = actionContext.Request.CreateResponse<object>(HttpStatusCode.Unauthorized, new { Message = "Please set the 'Authorization' header to the token granted on login!" });
            }
            else if (token != null)
            {
                if (!TokenManager.ValidateLoginToken(token.ToString()))
                {
                    throw new UnauthorizedAccessException("The provided token is not valid!");
                    //actionContext.Response = actionContext.Request.CreateResponse<object>(HttpStatusCode.Unauthorized, new { Message = "The provided token is not valid!" });
                }
            }
        }

    }
}