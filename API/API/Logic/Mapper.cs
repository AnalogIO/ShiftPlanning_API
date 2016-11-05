using Data.Models;
using DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Logic
{
    public static class Mapper
    {
        public static EmployeeDTO Map(Employee employee)
        {
            return new EmployeeDTO { Id = employee.Id, FirstName = employee.FirstName, LastName = employee.LastName };
        }

        public static IEnumerable<EmployeeDTO> Map(IEnumerable<Employee> employees)
        {
            var employeeDtos = new List<EmployeeDTO>();
            foreach(Employee employee in employees)
            {
                employeeDtos.Add(Map(employee));
            }
            return employeeDtos;
        }
    }
}