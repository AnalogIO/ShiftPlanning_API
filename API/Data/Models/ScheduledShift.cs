using System;
using System.Collections.Generic;

namespace Data.Models
{
    public class ScheduledShift
    {
        public int Id { get; set; }
        public int Day { get; set; }
        public TimeSpan Start { get; set; }
        public TimeSpan End { get; set; }
        public IEnumerable<Employee> Employees { get; set; }
    }
}