using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models
{
    public class Manager
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
        public virtual Institution Institution { get; set; }
        public virtual ICollection<Token> Tokens { get; set; }
    }
}