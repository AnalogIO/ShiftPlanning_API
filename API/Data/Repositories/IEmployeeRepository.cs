using System.Collections.Generic;
using Data.Models;

namespace Data.Repositories
{
    public interface IEmployeeRepository
    {
        Employee Create(Employee employee);
        IEnumerable<Employee> ReadFromInstitution(int institutionId);
        Employee Read(int id, int institutionId);
        int Update(Employee employee);
        void Delete(int id, int institutionId);
    }
}