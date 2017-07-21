using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class Preference
    {
        public int Id { get; set; }
        public int Priority { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual ScheduledShift ScheduledShift { get; set; }
    }
}
