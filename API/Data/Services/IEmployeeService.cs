using System.Collections.Generic;
using Data.Models;
using DataTransferObjects.Employee;

namespace Data.Services
{
    public interface IEmployeeService
    {
        Employee CreateEmployee(CreateEmployeeDTO employeeDto, Manager manager);
        void DeleteEmployee(int employeeId, Manager manager);
        Employee GetEmployee(int id, int institutionId);
        Employee GetEmployee(int id, string shortKey);
        IEnumerable<Employee> GetEmployees(int institutionId);
        IEnumerable<Employee> GetEmployees(string shortKey);
        Employee UpdateEmployee(int employeeId, UpdateEmployeeDTO employeeDto, Manager manager);
    }
}