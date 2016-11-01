using System.Collections.Generic;
using System.Linq;
using Data.Models;
using CheckIn = Data.Npgsql.Models.CheckIn;

namespace Data.Npgsql.Mapping
{
    public class CheckInMapMany : IMapMany<CheckIn, Data.Models.CheckIn>
    {
        public IEnumerable<Data.Models.CheckIn> Map(IEnumerable<CheckIn> source)
        {
            return source.Select(ci => new Data.Models.CheckIn
            {
                Id = ci.Id,
                Time = ci.Time,
                Employee = new Employee
                {
                    Id = ci.Employee.Id,
                    Email = ci.Employee.Email,
                    FirstName = ci.Employee.FirstName,
                    LastName = ci.Employee.LastName,
                    EmployeeTitle = new EmployeeTitle
                    {
                        Id = ci.Employee.EmployeeTitle.Id,
                        Title = ci.Employee.EmployeeTitle.Title,
                        Institution = new Institution
                        {
                            Id = ci.Employee.EmployeeTitle.Institution.Id,
                            Name = ci.Employee.EmployeeTitle.Institution.Name,
                            ApiKey = ci.Employee.EmployeeTitle.Institution.ApiKey
                        }
                    },
                    Institution = new Institution
                    {
                        Id = ci.Employee.Institution.Id,
                        Name = ci.Employee.Institution.Name,
                        ApiKey = ci.Employee.Institution.ApiKey
                    }
                }
            });
        }
    }
}
