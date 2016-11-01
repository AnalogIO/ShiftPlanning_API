using System.Collections.Generic;
using System.Linq;
using Data.Models;
using Employee = Data.Npgsql.Models.Employee;

namespace Data.Npgsql.Mapping
{
    public class EmployeeMapMany : IMapMany<Employee, Data.Models.Employee>
    {
        public IEnumerable<Data.Models.Employee> Map(IEnumerable<Employee> source)
        {
            return source.Select(e => new Data.Models.Employee
            {
                Id = e.Id,
                Email = e.Email,
                FirstName = e.FirstName,
                LastName = e.LastName,
                EmployeeTitle = new EmployeeTitle
                {
                    Id = e.EmployeeTitle.Id,
                    Title = e.EmployeeTitle.Title,
                    Institution = new Institution
                    {
                        Id = e.EmployeeTitle.Institution.Id,
                        Name = e.EmployeeTitle.Institution.Name,
                        ApiKey = e.EmployeeTitle.Institution.ApiKey
                    }
                },
                Institution = new Institution
                {
                    Id = e.Institution.Id,
                    Name = e.Institution.Name,
                    ApiKey = e.Institution.ApiKey
                }
            });
        }
    }
}