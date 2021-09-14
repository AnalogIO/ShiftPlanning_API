﻿using System.ComponentModel.DataAnnotations;

namespace ShiftPlanning.DTOs.Employee
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
        /// The title of the employee.
        /// </summary>
        public string EmployeeTitle { get; set; }

        /// <summary>
        /// A flag saying if the employee is currently active or not.
        /// </summary>
        [Required]
        public bool Active { get; set; }

        /// <summary>
        /// A base64 encoding of the profile picture.
        /// 
        /// May be left empty.
        /// </summary>
        public string ProfilePhoto { get; set; }


        /// <summary>
        /// The amount of shifts you want to take on a schedule
        /// </summary>
        [Required]
        public int WantShifts { get; set; }

        /// <summary>
        /// The old password of the employee.
        /// </summary>
        public string OldPassword { get; set; }

        /// <summary>
        /// The new password of the employee.
        /// </summary>
        public string NewPassword { get; set; }
    }
}