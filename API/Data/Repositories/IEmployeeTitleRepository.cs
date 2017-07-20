using System.Collections.Generic;
using Data.Models;

namespace Data.Repositories
{
    public interface IEmployeeTitleRepository
    {
        EmployeeTitle Create(EmployeeTitle employeeTitle);
        IEnumerable<EmployeeTitle> ReadFromOrganization(int organizationId);
        EmployeeTitle Read(int id, int organizationId);
        int Update(EmployeeTitle employeeTitle);
        void Delete(int id, int organizationId);
    }
}
