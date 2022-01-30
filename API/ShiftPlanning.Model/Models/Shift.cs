using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShiftPlanning.Model.Models
{
    public class Shift
    {
        [Key]
        public int Id { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public virtual ICollection<Employee> Employee_ { get; set; }
        public virtual ICollection<CheckIn> CheckIns { get; set; }
        [ForeignKey("Organization_Id")]
        public virtual Organization Organization { get; set; }
        [ForeignKey("Schedule_Id")]
        public virtual Schedule Schedule { get; set; }
    }
}