using System.Collections.Generic;
using ShiftPlanning.Model.Models;

namespace ShiftPlanning.WebApi.Services
{
    public interface IFriendshipService
    {
        void DeleteFriendship(Employee employee, int friendshipId);
        IEnumerable<Friendship> GetFriendships(Employee employee);
        Friendship CreateFriendship(Employee employee, int friendId);
    }
}
