using API.Models;
using API.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Data
{
    public interface IEmployeeRepository
    {
        Employee Create(Employee employee);
        List<Employee> ReadFromInstitution(int institutionId);
        Employee Read(int id, int institutionId);
        int Update(Employee employee);
        void Delete(int id, int institutionId);
    }
}