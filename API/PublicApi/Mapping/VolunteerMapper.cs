using System.Collections.Generic;
using System.Linq;
using Data.Models;
using DataTransferObjects.Public.Employees;

namespace PublicApi.Mapping
{
    public class VolunteerMapper : IVolunteerMapper
    {
        public IEnumerable<EmployeeDTO> Map(IEnumerable<Employee> employees)
        {
            return employees.Select(emp => new EmployeeDTO
            {
                Name = emp.FirstName,
                Title = emp.EmployeeTitle.Title,
                Photo = emp.Photo?.Data
            });
        }

        public EmployeeDTO Map(Employee employee)
        {
            return new EmployeeDTO
            {
                Name = employee.FirstName,
                Title = employee.EmployeeTitle.Title,
                Photo = employee.Photo?.Data
            };
        }
    }
}