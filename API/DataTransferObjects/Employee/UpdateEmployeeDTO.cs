using Microsoft.Build.Framework;

namespace DataTransferObjects.Employee
{
    public class UpdateEmployeeDTO
    {
        /// <summary>
        /// The email of the employee.
        /// </summary>
        [Required]
        public string Email { get; set; }

        /// <summary>
        /// The first name of the employee.
        /// </summary>
        [Required]
        public string FirstName { get; set; }

        /// <summary>
        /// The last name of the employee.
        /// </summary>
        [Required]
        public string LastName { get; set; }

        /// <summary>
        /// The title id of the employee.
        /// </summary>
        public int EmployeeTitleId { get; set; }

        /// <summary>
        /// A flag saying if the employee is currently active or not.
        /// </summary>
        [Required]
        public bool Active { get; set; }
    }
}