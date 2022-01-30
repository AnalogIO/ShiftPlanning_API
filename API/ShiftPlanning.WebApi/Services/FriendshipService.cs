using System.Collections.Generic;
using System.Linq;
using ShiftPlanning.Model.Models;
using ShiftPlanning.WebApi.Repositories;

namespace ShiftPlanning.WebApi.Services
{
    public class FriendshipService : IFriendshipService
    {
        private readonly IFriendshipRepository _friendshipRepository;
        private readonly IEmployeeRepository _employeeRepository;

        public FriendshipService(IFriendshipRepository friendshipRepository, IEmployeeRepository employeeRepository)
        {
            _friendshipRepository = friendshipRepository;
            _employeeRepository = employeeRepository;
        }

        public Friendship CreateFriendship(Employee employee, int friendId)
        {
            if (employee.Friendships.FirstOrDefault(f => f.Friend_Id == friendId) != null) return null; // if a friendship already exist

            var friend = _employeeRepository.Read(friendId, employee.Organization.Id);
            if (friend == null) return null;

            var friendship = new Friendship { Employee = employee, Employee_Id = employee.Id, Friend_Id = friendId };

            return _friendshipRepository.CreateFrienship(friendship);
        }

        public void DeleteFriendship(Employee employee, int friendshipId)
        {
            var friendship = employee.Friendships.FirstOrDefault(f => f.Id == friendshipId);
            if (friendship == null) return;

            _friendshipRepository.DeleteFriendship(friendship);
        }

        public IEnumerable<Friendship> GetFriendships(Employee employee)
        {
            return _friendshipRepository.GetFriendships(employee.Id);
        }
    }
}