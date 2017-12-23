using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects.Friendship
{
    public class FriendshipDTO
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public int FriendId { get; set; }
    }
}
