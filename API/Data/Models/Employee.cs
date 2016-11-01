using System.Collections.Generic;

namespace Data.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public EmployeeTitle EmployeeTitle { get; set; }
        public virtual Institution Institution { get; set; }
    }
}