using System.Collections.Generic;
using Data.Models;
using DataTransferObjects.Employee;

namespace Data.Services
{
    public interface IEmployeeService
    {
        Employee CreateEmployee(CreateEmployeeDTO employeeDto, Manager manager, Photo photo);
        IEnumerable<Employee> CreateManyEmployees(CreateEmployeeDTO[] employeeDtos, Manager manager);
        void DeleteEmployee(int employeeId, Manager manager);
        Employee GetEmployee(int id, int organizationId);
        Employee GetEmployee(int id, string shortKey);
        IEnumerable<Employee> GetEmployees(int organizationId);
        IEnumerable<Employee> GetEmployees(string shortKey);
        Employee UpdateEmployee(int employeeId, UpdateEmployeeDTO employeeDto, Manager manager);
        void SetPhoto(int employeeId, int organizationId, Photo photo);
    }
}