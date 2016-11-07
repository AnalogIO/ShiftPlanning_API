using System.Collections.Generic;
using Data.Models;
using DataTransferObjects;

namespace API.Services
{
    public interface IEmployeeService
    {
        Employee CreateEmployee(CreateEmployeeDTO employeeDto, Manager manager);
        void DeleteEmployee(int employeeId, Manager manager);
        Employee GetEmployee(int id, Manager manager);
        IEnumerable<Employee> GetEmployees(Manager manager);
        Employee UpdateEmployee(int employeeId, UpdateEmployeeDTO employeeDto, Manager manager);
    }
}