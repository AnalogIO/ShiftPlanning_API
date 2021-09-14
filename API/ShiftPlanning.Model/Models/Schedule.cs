using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShiftPlanning.Model.Models
{
    public class Schedule
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int NumberOfWeeks { get; set; }
        public virtual ICollection<ScheduledShift> ScheduledShifts { get; set; }
        [ForeignKey("Organization_Id")]
        public virtual Organization Organization { get; set; }
        public virtual ICollection<Shift> Shifts { get; set; }
    }
}