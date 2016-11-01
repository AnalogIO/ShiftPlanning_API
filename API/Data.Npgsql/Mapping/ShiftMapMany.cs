using System.Collections.Generic;
using System.Linq;
using Data.Npgsql.Models;

namespace Data.Npgsql.Mapping
{
    public class ShiftMapMany : IMapMany<Shift, Data.Models.Shift>
    {
        public IEnumerable<Data.Models.Shift> Map(IEnumerable<Shift> source)
        {
            return source.Select(s => new Data.Models.Shift
            {
                Id = s.Id,
                Start = s.Start,
                End = s.End,
                CheckIns = s.CheckIns.Select(ci => new Data.Models.CheckIn
                {
                    Id = ci.Id,
                    Time = ci.Time,
                    Employee = new Data.Models.Employee
                    {
                        Id = ci.Employee.Id,
                        Email = ci.Employee.Email,
                        FirstName = ci.Employee.FirstName,
                        LastName = ci.Employee.LastName,
                        EmployeeTitle = new Data.Models.EmployeeTitle
                        {
                            Id = ci.Employee.EmployeeTitle.Id,
                            Title = ci.Employee.EmployeeTitle.Title,
                            Institution = new Data.Models.Institution
                            {
                                Id = ci.Employee.EmployeeTitle.Institution.Id,
                                Name = ci.Employee.EmployeeTitle.Institution.Name,
                                ApiKey = ci.Employee.EmployeeTitle.Institution.ApiKey
                            }
                        },
                        Institution = new Data.Models.Institution
                        {
                            Id = ci.Employee.Institution.Id,
                            Name = ci.Employee.Institution.Name,
                            ApiKey = ci.Employee.Institution.ApiKey
                        }
                    }
                }),
                Employees = s.Employees.Select(e => new Data.Models.Employee
                {
                    Id = e.Id,
                    Email = e.Email,
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    EmployeeTitle = new Data.Models.EmployeeTitle
                    {
                        Id = e.EmployeeTitle.Id,
                        Title = e.EmployeeTitle.Title,
                        Institution = new Data.Models.Institution
                        {
                            Id = e.EmployeeTitle.Institution.Id,
                            Name = e.EmployeeTitle.Institution.Name,
                            ApiKey = e.EmployeeTitle.Institution.ApiKey
                        }
                    },
                    Institution = new Data.Models.Institution
                    {
                        Id = e.Institution.Id,
                        Name = e.Institution.Name,
                        ApiKey = e.Institution.ApiKey
                    }
                }),
                Institution = new Data.Models.Institution
                {
                    Id = s.Institution.Id,
                    Name = s.Institution.Name,
                    ApiKey = s.Institution.ApiKey
                }
            });
        }
    }
}
