using System.Collections.Generic;
using System.Linq;
using ShiftPlanning.DTOs.Employee;
using ShiftPlanning.Model.Models;

namespace ShiftPlanning.WebApi.Helpers.Mappers
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
                EmployeeTitle = employee.EmployeeTitle,
                PhotoRef = employee.PhotoUrl
            };
        }
    }
}