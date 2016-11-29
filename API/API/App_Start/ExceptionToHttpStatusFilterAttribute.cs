using System;
using System.Data.Entity.Core;
using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;

namespace API
{
    public class ExceptionToHttpStatusFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            if (context.Exception is NotImplementedException)
            {
                context.Response = context.Request.CreateErrorResponse(HttpStatusCode.NotImplemented, context.Exception.Message);
            }
            else if (context.Exception is ObjectNotFoundException)
            {
                context.Response = context.Request.CreateErrorResponse(HttpStatusCode.NotFound, context.Exception.Message);
            }
        }
    }
}