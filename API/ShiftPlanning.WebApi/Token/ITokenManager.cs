using System.Collections.Generic;
using ShiftPlanning.Model.Models;

namespace ShiftPlanning.WebApi.Token
{
    public interface ITokenManager
    {
        string GenerateToken();
        string GenerateLoginToken(IEnumerable<Role> roles);
        bool ValidateLoginToken(string token);
        
    }
}