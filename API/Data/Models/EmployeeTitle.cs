﻿namespace Data.Models
{
    public class EmployeeTitle
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public virtual Organization Organization { get; set; }
    }
}