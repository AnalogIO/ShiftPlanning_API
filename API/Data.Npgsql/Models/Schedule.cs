using System.Collections.Generic;
using System.Linq;

namespace Data.Npgsql.Models
{
    public class Schedule
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int NumberOfWeeks { get; set; }
        public virtual ICollection<ScheduledShift> Shifts { get; set; }
        public virtual Institution Institution { get; set; }
    }
}