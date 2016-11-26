using System.Collections.Generic;
using Data.Models;
using Data.Repositories;
using DataTransferObjects.Employee;

namespace Data.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IInstitutionRepository _institutionRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IEmployeeTitleRepository _employeeTitleRepository;

        public EmployeeService(IInstitutionRepository institutionRepository, IEmployeeRepository employeeRepository, IEmployeeTitleRepository employeeTitleRepository)
        {
            _institutionRepository = institutionRepository;
            _employeeRepository = employeeRepository;
            _employeeTitleRepository = employeeTitleRepository;
        }

        public IEnumerable<Employee> GetEmployees(int institutionId)
        {
            return _employeeRepository.ReadFromInstitution(institutionId);
        }

        public IEnumerable<Employee> GetEmployees(string shortKey)
        {
            return _employeeRepository.ReadFromInstitution(shortKey);
        }

        public Employee GetEmployee(int id, int institutionId)
        {
            return _employeeRepository.Read(id, institutionId);
        }

        public Employee GetEmployee(int id, string shortKey)
        {
            return _employeeRepository.Read(id, shortKey);
        }

        public Employee CreateEmployee(CreateEmployeeDTO employeeDto, Manager manager)
        {
            var employee = new Employee { Email = employeeDto.Email, FirstName = employeeDto.FirstName, LastName = employeeDto.LastName, Institution = manager.Institution, Active = true };
            var title = _employeeTitleRepository.Read(employeeDto.EmployeeTitleId, manager.Institution.Id);
            if (title == null) return null;
            employee.EmployeeTitle = title;
            return _employeeRepository.Create(employee);
        }

        public Employee UpdateEmployee(int employeeId, UpdateEmployeeDTO employeeDto, Manager manager)
        {
            var employee = _employeeRepository.Read(employeeId, manager.Institution.Id);
            if (employee != null)
            {
                employee.Email = employeeDto.Email;
                employee.FirstName = employeeDto.FirstName;
                employee.LastName = employeeDto.LastName;
                employee.Active = employeeDto.Active;

                var title = _employeeTitleRepository.Read(employeeDto.EmployeeTitleId, manager.Institution.Id);
                if (title != null) employee.EmployeeTitle = title;
                _employeeRepository.Update(employee);
                return employee;
            }
            return null;
        }

        public void DeleteEmployee(int employeeId, Manager manager)
        {
            _employeeRepository.Delete(employeeId, manager.Institution.Id);
        }
    }
}