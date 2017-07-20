﻿using System.Collections.Generic;

namespace Data.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public EmployeeTitle EmployeeTitle { get; set; }
        public bool Active { get; set; }
        public virtual ICollection<Shift> Shifts { get; set; }
        public virtual ICollection<CheckIn> CheckIns { get; set; }
        public virtual ICollection<ScheduledShift> ScheduledShifts { get; set; }
        public virtual Organization Organization { get; set; }
        public virtual Photo Photo { get; set; }
    }
}