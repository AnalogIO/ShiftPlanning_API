using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models
{
    public class Shift
    {
        public int Id { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public virtual ICollection<Employee> Employees { get; set; }
        public virtual ICollection<CheckIn> CheckIns { get; set; }
        public virtual Institution Institution { get; set; }
    }
}