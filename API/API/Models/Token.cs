using API.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models
{
    public class Token
    {
        public int Id { get; set; }
        public String TokenHash { get; set; }
        public virtual Manager Manager { get; set; }
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