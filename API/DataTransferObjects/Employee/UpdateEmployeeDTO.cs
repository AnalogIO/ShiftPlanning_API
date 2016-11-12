using Microsoft.Build.Framework;

namespace DataTransferObjects.Employee
{
    public class UpdateEmployeeDTO
    {
        /// <summary>
        /// The email of the employee. Leave empty string if the email should not be updated.
        /// </summary>
        [Required]
        public string Email { get; set; }

        /// <summary>
        /// The first name of the employee. Leave empty string if the first name should not be updated.
        /// </summary>
        [Required]
        public string FirstName { get; set; }

        /// <summary>
        /// The last name of the employee. Leave empty string if the last name should not be updated.
        /// </summary>
        [Required]
        public string LastName { get; set; }

        /// <summary>
        /// The title id of the employee. Leave as 0 if the title should not be updated.
        /// </summary>
        public int EmployeeTitleId { get; set; }

        /// <summary>
        /// A flag saying if the employee is currently active or not.
        /// </summary>
        [Required]
        public bool Active { get; set; }
    }
}