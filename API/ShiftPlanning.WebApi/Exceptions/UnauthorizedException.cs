namespace ShiftPlanning.WebApi.Exceptions
{
    public class UnauthorizedException : ApiException
    {
        public UnauthorizedException(string message, int statusCode = 401) : base(message, statusCode)
        {
        }
    }
}