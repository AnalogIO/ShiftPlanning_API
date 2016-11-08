using Data.Models;
using Data.Repositories;
using DataTransferObjects;
using System.Collections.Generic;

namespace API.Services
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