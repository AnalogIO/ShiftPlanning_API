using System.Collections.Generic;
using System.Linq;

namespace Data.Models
{
    public class Manager
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
        public virtual Organization Organization { get; set; }
        public virtual ICollection<Token> Tokens { get; set; }
    }
}