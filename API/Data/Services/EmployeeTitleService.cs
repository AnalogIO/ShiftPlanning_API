using System.Collections.Generic;
using System.Data;
using Data.Models;
using Data.Repositories;
using DataTransferObjects.EmployeeTitles;

namespace Data.Services
{
    public class EmployeeTitleService : IEmployeeTitleService
    {
        private readonly IEmployeeTitleRepository _employeeTitleRepository;

        public EmployeeTitleService(IEmployeeTitleRepository employeeTitleRepository)
        {
            _employeeTitleRepository = employeeTitleRepository;
        }

        public EmployeeTitle CreateEmployeeTitle(CreateEmployeeTitleDTO employeeTitleDto, Employee employee)
        {
            var employeeTitle = new EmployeeTitle { Title = employeeTitleDto.Title, Organization = employee.Organization };
            return _employeeTitleRepository.Create(employeeTitle);
        }

        public void DeleteEmployeeTitle(int employeeTitleId, Employee employee)
        {
            _employeeTitleRepository.Delete(employeeTitleId, employee.Organization.Id);
        }

        public EmployeeTitle GetEmployeeTitle(int id, Employee employee)
        {
            return _employeeTitleRepository.Read(id, employee.Organization.Id);
        }

        public IEnumerable<EmployeeTitle> GetEmployeeTitles(Employee employee)
        {
            return _employeeTitleRepository.ReadFromOrganization(employee.Organization.Id);
        }

        public EmployeeTitle UpdateEmployeeTitle(int employeeTitleId, UpdateEmployeeTitleDTO employeeTitleDto, Employee employee)
        {
            var employeeTitle = _employeeTitleRepository.Read(employeeTitleId, employee.Organization.Id);
            if (employeeTitle == null) throw new ObjectNotFoundException("Could not find a title corresponding to the given id");

            employeeTitle.Title = employeeTitleDto.Title;

            _employeeTitleRepository.Update(employeeTitle);

            return employeeTitle;
        }
    }
}