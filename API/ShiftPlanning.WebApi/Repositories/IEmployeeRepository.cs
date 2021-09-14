using System.Collections.Generic;
using ShiftPlanning.Model.Models;

namespace ShiftPlanning.WebApi.Repositories
{
    public interface IEmployeeRepository
    {
        Employee Create(Employee employee);
        IEnumerable<Employee> CreateMany(IEnumerable<Employee> employees);
        IEnumerable<Employee> ReadFromOrganization(int organizationId);
        IEnumerable<Employee> ReadFromOrganization(string shortKey);
        Employee Read(int id, int organizationId);
        Employee Read(int id, string shortKey);
        int Update(Employee employee);
        int UpdateMany(IEnumerable<Employee> employees);
        void Delete(int id, int organizationId);
        Employee Read(string token);
        Employee Login(string email, string password);
        void DeleteFriendship(Friendship friendship);
        IEnumerable<Role> GetRoles();
    }
}