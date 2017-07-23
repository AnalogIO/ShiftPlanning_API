using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Build.Framework;

namespace Data.Models
{
    public class Preference
    {
        public int Id { get; set; }
        public int Priority { get; set; }
        [Required]
        public virtual Employee Employee { get; set; }
        [Required]
        public virtual ScheduledShift ScheduledShift { get; set; }
    }
}
