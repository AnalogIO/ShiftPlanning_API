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
        Employee Create(RegisterDTO employee);
        List<Employee> Read();
        Employee Read(int id);
        int Update(Employee employee);
        void Delete(int id);
    }
}