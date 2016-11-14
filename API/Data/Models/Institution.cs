using System;
using System.Collections.Generic;

namespace Data.Models
{
    public class Institution
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortKey { get; set; }
        public string ApiKey { get; set; }
        // public TimeZone TimeZone { get; set; } removed for now
        public virtual ICollection<Manager> Managers { get; set; }
        public virtual ICollection<Employee> Employees { get; set; }
        public virtual ICollection<EmployeeTitle> EmployeeTitles { get; set; }
        public virtual ICollection<Schedule> Schedules { get; set; }
        public virtual ICollection<Shift> Shifts { get; set; }
    }
}