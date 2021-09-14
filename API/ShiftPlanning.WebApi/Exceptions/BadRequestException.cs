namespace ShiftPlanning.WebApi.Exceptions
{
    public class BadRequestException : ApiException
    {
        public BadRequestException(string message, int statusCode = 400) : base(message, statusCode)
        {
        }
    }
}