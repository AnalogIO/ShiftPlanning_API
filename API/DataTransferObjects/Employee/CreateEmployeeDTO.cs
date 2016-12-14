using Microsoft.Build.Framework;

namespace DataTransferObjects.Employee
{
    public class CreateEmployeeDTO
    {
        /// <summary>
        /// The email of the employee to be created
        /// </summary>
        [Required]
        public string Email { get; set; }

        /// <summary>
        /// The first name of the employee
        /// </summary>
        [Required]
        public string FirstName { get; set; }

        /// <summary>
        /// The last name of the employee
        /// </summary>
        [Required]
        public string LastName { get; set; }

        /// <summary>
        /// The title id of the employee.
        /// </summary>
        [Required]
        public int EmployeeTitleId { get; set; }

        /// <summary>
        /// A base64 encoding of the profile picture.
        /// 
        /// May be left empty.
        /// </summary>
        public string ProfilePhoto { get; set; }
    }
}