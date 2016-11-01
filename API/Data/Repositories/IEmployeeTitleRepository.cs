using System.Collections.Generic;
using Data.Models;

namespace Data.Repositories
{
    public interface IEmployeeTitleRepository
    {
        EmployeeTitle Create(EmployeeTitle employeeTitle);
        IEnumerable<EmployeeTitle> ReadFromInstitution(int institutionId);
        EmployeeTitle Read(int id, int institutionId);
        int Update(EmployeeTitle employeeTitle);
        void Delete(int id, int institutionId);
    }
}
