using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Data.Models
{
    public class Barista
    {
        public int Id { get; set; }
        public int IndexId { get; set; }
        public IEnumerable<Preference> Preferences { get; set; }
    }
}