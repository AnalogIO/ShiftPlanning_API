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
        EmployeeTitle Create(EmployeeTitle employeeTitle);
        List<EmployeeTitle> ReadFromInstitution(int institutionId);
        EmployeeTitle Read(int id, int institutionId);
        int Update(EmployeeTitle employeeTitle);
        void Delete(int id, int institutionId);
    }
}
