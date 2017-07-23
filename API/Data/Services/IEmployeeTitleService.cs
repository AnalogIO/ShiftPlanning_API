using System.Collections.Generic;
using Data.Models;
using DataTransferObjects.EmployeeTitles;

namespace Data.Services
{
    public interface IEmployeeTitleService
    {
        EmployeeTitle CreateEmployeeTitle(CreateEmployeeTitleDTO employeeTitleDto, Employee employee);
        void DeleteEmployeeTitle(int employeeTitleId, Employee employee);
        EmployeeTitle GetEmployeeTitle(int id, Employee employee);
        IEnumerable<EmployeeTitle> GetEmployeeTitles(Employee employee);
        EmployeeTitle UpdateEmployeeTitle(int employeeTitleId, UpdateEmployeeTitleDTO employeeTitleDto, Employee employee);
    }
}
