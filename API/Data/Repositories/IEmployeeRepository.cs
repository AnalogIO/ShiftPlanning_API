using System.Collections.Generic;
using Data.Models;

namespace Data.Repositories
{
    public interface IEmployeeRepository
    {
        Employee Create(Employee employee);
        IEnumerable<Employee> ReadFromInstitution(int institutionId);
        IEnumerable<Employee> ReadFromInstitution(string shortKey);
        Employee Read(int id, int institutionId);
        Employee Read(int id, string shortKey);
        int Update(Employee employee);
        void Delete(int id, int institutionId);
    }
}