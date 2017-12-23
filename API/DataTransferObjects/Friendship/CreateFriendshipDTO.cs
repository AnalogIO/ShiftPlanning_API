using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace DataTransferObjects.Friendship
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
