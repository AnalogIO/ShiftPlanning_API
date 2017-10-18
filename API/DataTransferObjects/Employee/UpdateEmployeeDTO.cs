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

        /// <summary>
        /// A base64 encoding of the profile picture.
        /// 
        /// May be left empty.
        /// </summary>
        public string ProfilePhoto { get; set; }

        /// <summary>
        /// An array stating the ids of the user's friends
        /// </summary>
        [Required]
        public int[] FriendshipIds { get; set; }

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