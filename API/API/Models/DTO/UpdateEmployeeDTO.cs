using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models.DTO
{
    public class UpdateEmployeeDTO
    {
        /// <summary>
        /// The email of the employee. Leave empty string if the email should not be updated.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// The first name of the employee. Leave empty string if the first name should not be updated.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// The last name of the employee. Leave empty string if the last name should not be updated.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// The title id of the employee. Leave as 0 if the title should not be updated.
        /// </summary>
        public int EmployeeTitleId { get; set; }
    }
}