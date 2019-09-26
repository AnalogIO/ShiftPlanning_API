using Data.Token;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Models
{
    public class Token
    {
        [Key]
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