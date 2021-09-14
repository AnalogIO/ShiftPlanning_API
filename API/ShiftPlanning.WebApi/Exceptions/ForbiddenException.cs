using System;

namespace ShiftPlanning.WebApi.Exceptions
{
    public class ForbiddenException : ApiException
    {
        public ForbiddenException(string message, int statusCode = 403) : base(message, statusCode)
        {
        }
    }
}
