using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace API.Models.DTO
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
        public int EmployeeTitleId { get; set; }
    }
}