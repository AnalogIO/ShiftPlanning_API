
using System.ComponentModel.DataAnnotations;

namespace ShiftPlanning.DTOs.Employee
{
    public class EmployeeLoginDTO
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}