using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Filters;

namespace API.Logic
{
    public class AdminFilter : AuthorizationFilterAttribute
    {
        /// <summary>
        /// OnAuthorization is called whenever a method has the data annotation "[AdminFilter]".
        /// Checks if the user is authorized.
        /// </summary>
        /// <param name="actionContext"></param>
        public override void OnAuthorization(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            var apikey = actionContext.Request.Headers.Authorization;
            if (apikey == null)
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
            }
            else if (apikey != null)
            {
                if (!AuthManager.AuthenticateToken(apikey.ToString()))
                {
                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                }
            }
        }

    }
}