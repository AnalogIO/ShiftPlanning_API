namespace ShiftPlanning.WebApi.Token
{
    public interface ITokenManager
    {
        string GenerateToken();
        string GenerateLoginToken();
        bool ValidateLoginToken(string token);
        
    }
}