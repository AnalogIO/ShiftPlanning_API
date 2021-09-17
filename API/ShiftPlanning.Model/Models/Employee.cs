using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShiftPlanning.Model.Models
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmployeeTitle { get; set; }
        public bool Active { get; set; }
        public int WantShifts { get; set; }
        public ICollection<Shift> Shift_ { get; set; }
        public ICollection<CheckIn> CheckIns { get; set; }
        public ICollection<EmployeeAssignment> EmployeeAssignments { get; set; }
        public ICollection<Preference> Preferences { get; set; }
        public ICollection<Friendship> Friendships { get; set; }
        
        [ForeignKey("Organization_Id")]
        public Organization Organization { get; set; }
        public string PhotoUrl { get; set; }
        
        public ICollection<Role> Role_ { get; set; }
        [ForeignKey("Employee_Id")]
        public ICollection<Token> Tokens { get; set; }
        public int PodioId { get; set; }
    }
}