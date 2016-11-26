using System.Collections.Generic;
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

        public EmployeeTitle CreateEmployeeTitle(CreateEmployeeTitleDTO employeeTitleDto, Manager manager)
        {
            var employeeTitle = new EmployeeTitle { Title = employeeTitleDto.Title, Organization = manager.Organization };
            return _employeeTitleRepository.Create(employeeTitle);
        }

        public void DeleteEmployeeTitle(int employeeTitleId, Manager manager)
        {
            _employeeTitleRepository.Delete(employeeTitleId, manager.Organization.Id);
        }

        public EmployeeTitle GetEmployeeTitle(int id, Manager manager)
        {
            return _employeeTitleRepository.Read(id, manager.Organization.Id);
        }

        public IEnumerable<EmployeeTitle> GetEmployeeTitles(Manager manager)
        {
            return _employeeTitleRepository.ReadFromOrganization(manager.Organization.Id);
        }

        public EmployeeTitle UpdateEmployeeTitle(int employeeTitleId, UpdateEmployeeTitleDTO employeeTitleDto, Manager manager)
        {
            var employeeTitle = _employeeTitleRepository.Read(employeeTitleId, manager.Organization.Id);
            if (employeeTitle != null)
            {
                employeeTitle.Title = employeeTitleDto.Title;
                return (_employeeTitleRepository.Update(employeeTitle) > 0) ? employeeTitle : null;
            }
            return null;
        }
    }
}