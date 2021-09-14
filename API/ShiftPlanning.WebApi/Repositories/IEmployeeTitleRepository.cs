using System.Collections.Generic;
using ShiftPlanning.Model.Models;

namespace ShiftPlanning.WebApi.Repositories
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
