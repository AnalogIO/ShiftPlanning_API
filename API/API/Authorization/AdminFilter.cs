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
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
            }
            else if (token != null)
            {
                if (!TokenManager.ValidateLoginToken(token.ToString()))
                {
                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                }
            }
        }

    }
}