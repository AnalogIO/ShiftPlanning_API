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
            if (context.Exception == null) return;
            Func<ObjectResult> contextResult = context.Exception switch
            {
                ApiException exception => () => new ObjectResult(exception.Message)
                {
                    StatusCode = exception.StatusCode,
                },
                Exception => () => new ObjectResult("An unknown error occured, try again later. If the error persists, contact support")
                {
                    StatusCode = 500,
                }
            };
            context.Result = contextResult.Invoke();
            context.ExceptionHandled = true;
        }

        public int Order { get; } = int.MaxValue - 10;
    }
}