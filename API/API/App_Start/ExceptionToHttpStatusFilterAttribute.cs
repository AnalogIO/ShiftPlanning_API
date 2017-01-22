using System;
using System.Data;
using System.IdentityModel;
using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;
using API.Exceptions;
using Data.Exceptions;

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
            else if (context.Exception is BadRequestException)
            {
                context.Response = context.Request.CreateErrorResponse(HttpStatusCode.BadRequest, context.Exception.Message);
            }
            else if (context.Exception is UnauthorizedAccessException)
            {
                context.Response = context.Request.CreateErrorResponse(HttpStatusCode.Unauthorized, context.Exception.Message);
            }
            else if (context.Exception is ForbiddenException)
            {
                context.Response = context.Request.CreateErrorResponse(HttpStatusCode.Forbidden, context.Exception.Message);
            }
        }
    }
}