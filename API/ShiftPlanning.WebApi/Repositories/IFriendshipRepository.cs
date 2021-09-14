using System.Collections.Generic;
using ShiftPlanning.Model.Models;

namespace ShiftPlanning.WebApi.Repositories
{
    public interface IFriendshipRepository
    {
        void DeleteFriendship(Friendship friendship);
        IEnumerable<Friendship> GetFriendships(int employeeId);
        Friendship CreateFrienship(Friendship friendship);
    }
}