﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Models
{
    public class Organization
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortKey { get; set; }
        public string ApiKey { get; set; }
        // public TimeZone TimeZone { get; set; } removed for now

        public virtual Photo DefaultPhoto { get; set; }
        public virtual ICollection<Photo> Photos { get; set; }
        public virtual ICollection<Employee> Employees { get; set; }
        public virtual ICollection<EmployeeTitle> EmployeeTitles { get; set; }
        public virtual ICollection<Schedule> Schedules { get; set; }
        public virtual ICollection<Shift> Shifts { get; set; }
        public virtual EmailSettings EmailSettings { get; set; }
    }
}