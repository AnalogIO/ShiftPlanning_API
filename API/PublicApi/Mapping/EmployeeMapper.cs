using System.Collections.Generic;
using System.Linq;
using System.Web;
using Data.Models;
using DataTransferObjects.Public.Employees;
using PublicApi.Controllers;

namespace PublicApi.Mapping
{
    public class EmployeeMapper : IVolunteerMapper
    {
        public IEnumerable<EmployeeDTO> Map(IEnumerable<Employee> employees)
        {
            return employees.Select(Map);
        }

        public EmployeeDTO Map(Employee employee)
        {
            return new EmployeeDTO
            {
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Title = employee.EmployeeTitle.Title,
                PhotoRef = employee.PhotoUrl
            };
        }
    }
}