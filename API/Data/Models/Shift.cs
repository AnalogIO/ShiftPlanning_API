using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Models
{
    public class Shift
    {
        [Key]
        public int Id { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public virtual ICollection<Employee> Employees { get; set; }
        public virtual ICollection<CheckIn> CheckIns { get; set; }
        public virtual Organization Organization { get; set; }
        public virtual Schedule Schedule { get; set; }
    }
}