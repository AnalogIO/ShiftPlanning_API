using Data.Models;
using DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
