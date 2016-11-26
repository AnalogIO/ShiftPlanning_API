using System.Collections.Generic;
using Data.Models;

namespace Data.Repositories
{
    public interface IEmployeeRepository
    {
        Employee Create(Employee employee);
        IEnumerable<Employee> CreateMany(IEnumerable<Employee> employees);
        IEnumerable<Employee> ReadFromOrganization(int organizationId);
        IEnumerable<Employee> ReadFromOrganization(string shortKey);
        Employee Read(int id, int organizationId);
        Employee Read(int id, string shortKey);
        int Update(Employee employee);
        void Delete(int id, int organizationId);
    }
}