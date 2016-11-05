using Data.Models;
using Data.Repositories;
using DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Services
{
    public class EmployeeService
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

        public IEnumerable<Employee> GetEmployees(Manager manager)
        {
            return _employeeRepository.ReadFromInstitution(manager.Institution.Id);
        }

        public Employee GetEmployee(int id, Manager manager)
        {
            return _employeeRepository.Read(id, manager.Institution.Id);
        }

        public Employee CreateEmployee(CreateEmployeeDTO employeeDto, Manager manager)
        {
            var employee = new Employee { Email = employeeDto.Email, FirstName = employeeDto.FirstName, LastName = employeeDto.LastName, Institution = manager.Institution };
            var title = _employeeTitleRepository.Read(employeeDto.EmployeeTitleId, manager.Institution.Id);
            if (title != null) employee.EmployeeTitle = title;
            return _employeeRepository.Create(employee);
        }

        public Employee UpdateEmployee(int employeeId, UpdateEmployeeDTO employeeDto, Manager manager)
        {
            var employee = _employeeRepository.Read(employeeId, manager.Institution.Id);
            if (employee != null)
            {
                if (employeeDto.Email.Length > 0) employee.Email = employeeDto.Email;
                if (employeeDto.FirstName.Length > 0) employee.FirstName = employeeDto.FirstName;
                if (employeeDto.LastName.Length > 0) employee.LastName = employeeDto.LastName;
                var title = _employeeTitleRepository.Read(employeeDto.EmployeeTitleId, manager.Institution.Id);
                if (title != null) employee.EmployeeTitle = title;
                return _employeeRepository.Update(employee) > 0 ? employee : null;
            }
            return null;
        }

        public void DeleteEmployee(int employeeId, Manager manager)
        {
            _employeeRepository.Delete(employeeId, manager.Institution.Id);
        }
    }
}