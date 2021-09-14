using System.ComponentModel.DataAnnotations;

namespace ShiftPlanning.Model.Models
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
    }
}