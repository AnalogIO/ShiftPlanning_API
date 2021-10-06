using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ShiftPlanning.WebApi.Exceptions
{
    public class HttpResponseExceptionFilter : IActionFilter, IOrderedFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            var contextResult = context.Exception switch
            {
                ApiException exception => new ObjectResult(exception.Message)
                {
                    StatusCode = exception.StatusCode,
                },
                Exception exception => new ObjectResult(exception.Message)
                {
                    StatusCode = 500,
                }
            };
            context.Result = contextResult;
            context.ExceptionHandled = true;
        }

        public int Order { get; } = int.MaxValue - 10;
    }
}