using System.ComponentModel.DataAnnotations;

namespace ShiftPlanning.DTOs.Friendship
{
    public class CreateFriendshipDTO
    {
        /// <summary>
        /// The employee id of the employee to add as a friend
        /// </summary>
        [Required]
        public int FriendEmployeeId { get; set; }
    }
}
