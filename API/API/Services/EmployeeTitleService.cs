using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Data.Models;
using DataTransferObjects;
using Data.Repositories;

namespace API.Services
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
            var employeeTitle = new EmployeeTitle { Title = employeeTitleDto.Title, Institution = manager.Institution };
            return _employeeTitleRepository.Create(employeeTitle);
        }

        public void DeleteEmployeeTitle(int employeeTitleId, Manager manager)
        {
            _employeeTitleRepository.Delete(employeeTitleId, manager.Institution.Id);
        }

        public EmployeeTitle GetEmployeeTitle(int id, Manager manager)
        {
            return _employeeTitleRepository.Read(id, manager.Institution.Id);
        }

        public IEnumerable<EmployeeTitle> GetEmployeeTitles(Manager manager)
        {
            return _employeeTitleRepository.ReadFromInstitution(manager.Institution.Id);
        }

        public EmployeeTitle UpdateEmployeeTitle(int employeeTitleId, UpdateEmployeeTitleDTO employeeTitleDto, Manager manager)
        {
            var employeeTitle = _employeeTitleRepository.Read(employeeTitleId, manager.Institution.Id);
            if (employeeTitle != null)
            {
                employeeTitle.Title = employeeTitleDto.Title;
                return (_employeeTitleRepository.Update(employeeTitle) > 0) ? employeeTitle : null;
            }
            return null;
        }
    }
}