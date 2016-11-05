using System.Collections.Generic;
using System.Linq;

namespace Data.Models
{
    public class Schedule
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int NumberOfWeeks { get; set; }
        public virtual ICollection<ScheduledShift> ScheduledShifts { get; set; }
        public virtual Institution Institution { get; set; }
        public virtual ICollection<Shift> Shifts { get; set; }
    }
}