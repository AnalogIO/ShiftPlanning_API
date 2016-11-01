using System;
using Data.Token;

namespace Data.Models
{
    public class Token
    {
        public int Id { get; set; }
        public string TokenHash { get; set; }
        public Token(string tokenHash)
        {
            TokenHash = tokenHash;
        }
        public Token()
        {
            TokenHash = TokenManager.GenerateLoginToken();
        }
    }
}