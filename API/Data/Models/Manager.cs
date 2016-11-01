using System.Collections.Generic;

namespace Data.Models
{
    public class Manager
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
        public virtual Institution Institution { get; set; }
        public virtual IEnumerable<Token> Tokens { get; set; }
    }
}