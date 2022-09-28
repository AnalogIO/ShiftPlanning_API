using System.Collections.Generic;
using System.Threading.Tasks;
using ShiftPlanning.DTOs.Employee;
using ShiftPlanning.Model.Models;

namespace ShiftPlanning.WebApi.Services
{
    public interface IEmployeeService
    {
        Employee CreateEmployee(CreateEmployeeDTO employeeDto, Employee employee);
        Employee CreateEmployeeFromPodio(CreateEmployeeDTO employeeDto, Organization organization);
        IEnumerable<Employee> CreateManyEmployees(CreateEmployeeDTO[] employeeDtos, Employee employee);
        void DeleteEmployee(int employeeId, Employee employee);
        Employee GetEmployee(int id, int organizationId);
        Employee GetEmployee(int id, string shortKey);
        IEnumerable<Employee> GetEmployees(int organizationId);
        IEnumerable<Employee> GetEmployees(string shortKey);
        IEnumerable<Employee> GetEmployeesByActivity(int organizationId, bool active);
        IEnumerable<Employee> GetEmployeesByActivity(string shortKey, bool active);

        Employee UpdateEmployee(UpdateEmployeeDTO employeeDto, Employee employee, Photo photo);
        Employee Login(string email, string password);
        Friendship CreateFriendship(Employee employee, int friendId);
        void DeleteFriendship(Employee employee, int friendId);
        void ResetPassword(int id, int organizationId);
        Task<int> SyncEmployees(string shortKey);
    }
}