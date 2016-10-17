using API.Models;
using API.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Data
{
    public interface IEmployeeTitleRepository
    {
        EmployeeTitle Create(CreateEmployeeTitleDTO employeeTitle);
        List<EmployeeTitle> Read();
        EmployeeTitle Read(int id);
        int Update(EmployeeTitle employeeTitle);
        void Delete(int id);
    }
}
