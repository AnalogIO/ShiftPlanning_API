using System.Collections.Generic;
using Data.Models;

namespace Data.Repositories
{
    public interface IFriendshipRepository
    {
        void DeleteFriendship(Friendship friendship);
        IEnumerable<Friendship> GetFriendships(int employeeId);
        Friendship CreateFrienship(Friendship friendship);
    }
}