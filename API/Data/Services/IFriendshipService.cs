using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Services
{
    public interface IFriendshipService
    {
        void DeleteFriendship(Employee employee, int friendshipId);
        IEnumerable<Friendship> GetFriendships(Employee employee);
        Friendship CreateFriendship(Employee employee, int friendId);
    }
}
