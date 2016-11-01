using System;
using System.Collections.Generic;

namespace Data.Models
{
    public class Shift
    {
        public int Id { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public virtual IEnumerable<Employee> Employees { get; set; }
        public virtual IEnumerable<CheckIn> CheckIns { get; set; }
        public virtual Institution Institution { get; set; }
    }
}