using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Data.Models
{
    public class Shift
    {
        public int Id { get; set; }
        public int MaxOnShift { get; set; }
        public int MinOnshift { get; set; }
        public int IndexId { get; set; }
    }
}