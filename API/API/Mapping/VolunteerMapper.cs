using Data.Models;
using DataTransferObjects.Volunteers;
using System.Collections.Generic;
using System.Linq;

namespace API.Mapping
{
    public class VolunteerMapper : IVolunteerMapper
    {
        public IEnumerable<VolunteerDTO> Map(IEnumerable<Employee> employees)
        {
            return employees.Select(emp => new VolunteerDTO
            {
                Name = emp.FirstName,
                Title = emp.EmployeeTitle.Title,
                Photo = emp.Photo?.Data
            });
        }

        public VolunteerDTO Map(Employee employee)
        {
            return new VolunteerDTO
            {
                Name = employee.FirstName,
                Title = employee.EmployeeTitle.Title,
                Photo = employee.Photo?.Data
            };
        }
    }
}