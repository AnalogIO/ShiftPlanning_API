using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models
{
    public class CheckIn
    {
        public int Id { get; set; }
        public DateTime Time { get; set; }
        public virtual Employee Employee { get; set; }
    }
}