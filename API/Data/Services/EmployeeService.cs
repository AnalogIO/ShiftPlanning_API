using System.Collections.Generic;
using Data.Models;
using Data.Repositories;
using DataTransferObjects.Employee;
using System.Linq;
using System;
using System.Data;
using System.IdentityModel;
using Data.Exceptions;

namespace Data.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IOrganizationRepository _organizationRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IEmployeeTitleRepository _employeeTitleRepository;

        public EmployeeService(IOrganizationRepository organizationRepository, IEmployeeRepository employeeRepository, IEmployeeTitleRepository employeeTitleRepository)
        {
            _organizationRepository = organizationRepository;
            _employeeRepository = employeeRepository;
            _employeeTitleRepository = employeeTitleRepository;
        }

        public IEnumerable<Employee> GetEmployees(int organizationId)
        {
            return _employeeRepository.ReadFromOrganization(organizationId);
        }

        public IEnumerable<Employee> GetEmployees(string shortKey)
        {
            return _employeeRepository.ReadFromOrganization(shortKey);
        }

        public Employee GetEmployee(int id, int institutionId)
        {
            return _employeeRepository.Read(id, institutionId);
        }

        public Employee GetEmployee(int id, string shortKey)
        {
            return _employeeRepository.Read(id, shortKey);
        }

        public Employee CreateEmployee(CreateEmployeeDTO employeeDto, Manager manager, Photo photo)
        {
            var employee = new Employee
            {
                Email = employeeDto.Email,
                FirstName = employeeDto.FirstName,
                LastName = employeeDto.LastName,
                Organization = manager.Organization,
                Active = true,
                Photo = photo
            };
            var title = _employeeTitleRepository.Read(employeeDto.EmployeeTitleId, manager.Organization.Id);
            if (title == null) throw new ObjectNotFoundException("Could not find a title corresponding to the given id");
            employee.EmployeeTitle = title;
            return _employeeRepository.Create(employee);
        }

        public Employee UpdateEmployee(int employeeId, UpdateEmployeeDTO employeeDto, Manager manager, Photo photo)
        {
            var employee = _employeeRepository.Read(employeeId, manager.Organization.Id);
            if (employee != null)
            {
                employee.Email = employeeDto.Email;
                employee.FirstName = employeeDto.FirstName;
                employee.LastName = employeeDto.LastName;
                employee.Active = employeeDto.Active;

                if (photo != null)
                {
                    employee.Photo = photo;
                }

                var title = _employeeTitleRepository.Read(employeeDto.EmployeeTitleId, manager.Organization.Id);
                if (title != null) employee.EmployeeTitle = title;
                _employeeRepository.Update(employee);
                return employee;
            }
            throw new ObjectNotFoundException("Could not find an employee corresponding to the given id");
        }

        public IEnumerable<Employee> CreateManyEmployees(CreateEmployeeDTO[] employeeDtos, Manager manager)
        {
            return _employeeRepository.CreateMany(employeeDtos.Select(employeeDto => new Employee {
                Email = employeeDto.Email,
                FirstName = employeeDto.FirstName,
                LastName = employeeDto.LastName,
                Organization = manager.Organization,
                Active = true,
                EmployeeTitle = _employeeTitleRepository.Read(employeeDto.EmployeeTitleId, manager.Organization.Id) }));
        }

        public void DeleteEmployee(int employeeId, Manager manager)
        {
            _employeeRepository.Delete(employeeId, manager.Organization.Id);
        }

        public void SetPhoto(int employeeId, int organizationId, Photo photo)
        {
            var employee = _employeeRepository.Read(employeeId, organizationId);

            employee.Photo = photo;

            _employeeRepository.Update(employee);
        }
    }
}