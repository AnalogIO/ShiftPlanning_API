using System;
using System.Collections.Generic;
using System.Linq;

namespace Data.Models
{
    public class ScheduledShift
    {
        public int Id { get; set; }
        public int Day { get; set; }
        public TimeSpan Start { get; set; }
        public TimeSpan End { get; set; }
        public int MaxOnShift { get; set; }
        public int MinOnShift { get; set; }
        public virtual ICollection<Employee> Employees { get; set; }
        public virtual ICollection<Preference> Preferences { get; set; }
        public virtual Schedule Schedule { get; set; }
    }
}