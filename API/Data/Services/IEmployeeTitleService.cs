using System.Collections.Generic;
using Data.Models;
using DataTransferObjects.EmployeeTitles;

namespace Data.Services
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
