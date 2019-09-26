using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Data.Models
{
    public class ScheduledShift
    {
        [Key]
        public int Id { get; set; }
        public int Day { get; set; }
        public TimeSpan Start { get; set; }
        public TimeSpan End { get; set; }
        public int MaxOnShift { get; set; }
        public int MinOnShift { get; set; }
        public virtual ICollection<EmployeeAssignment> EmployeeAssignments { get; set; }
        public virtual ICollection<Preference> Preferences { get; set; }
        public virtual Schedule Schedule { get; set; }
    }
}