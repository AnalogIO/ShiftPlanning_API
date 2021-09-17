using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShiftPlanning.Model.Models
{
     [Table("Tokens")]
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