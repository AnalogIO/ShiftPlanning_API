using Microsoft.Build.Framework;

namespace DataTransferObjects.Employee
{
    public class EmployeeLoginDTO
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}