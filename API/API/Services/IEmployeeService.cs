using System.Collections.Generic;
using Data.Models;
using DataTransferObjects;

namespace API.Services
{
    public interface IEmployeeService
    {
        Employee CreateEmployee(CreateEmployeeDTO employeeDto, Manager manager);
        void DeleteEmployee(int employeeId, Manager manager);
        Employee GetEmployee(int id, int institutionId);
        IEnumerable<Employee> GetEmployees(int institutionId);
        Employee UpdateEmployee(int employeeId, UpdateEmployeeDTO employeeDto, Manager manager);
    }
}