using System;
using System.Collections.Generic;

namespace Data.Models
{
    public class Shift
    {
        public int Id { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public virtual ICollection<Employee> Employees { get; set; }
        public virtual ICollection<CheckIn> CheckIns { get; set; }
        public virtual Organization Organization { get; set; }
        public virtual Schedule Schedule { get; set; }
    }
}