using Data.Models;
using DataTransferObjects.EmployeeTitles;
using System.Collections.Generic;

namespace API.Services
{
    public interface IEmployeeTitleService
    {
        EmployeeTitle CreateEmployeeTitle(CreateEmployeeTitleDTO employeeTitleDto, Manager manager);
        void DeleteEmployeeTitle(int employeeTitleId, Manager manager);
        EmployeeTitle GetEmployeeTitle(int id, Manager manager);
        IEnumerable<EmployeeTitle> GetEmployeeTitles(Manager manager);
        EmployeeTitle UpdateEmployeeTitle(int employeeTitleId, UpdateEmployeeTitleDTO employeeTitleDto, Manager manager);
    }
}
