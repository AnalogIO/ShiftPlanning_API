using System;
using System.Runtime.Serialization;

namespace ShiftPlanning.WebApi.Exceptions
{
    public class ObjectNotFoundException : ApiException
    {
        public ObjectNotFoundException(string message, int statusCode = 404) : base(message, statusCode)
        {
        }
    }
}