using System.Collections.Generic;
using System.Linq;
using Data.Npgsql.Models;

namespace Data.Npgsql.Mapping
{
    public class ScheduledShiftMapMany : IMapMany<ScheduledShift, Data.Models.ScheduledShift>
    {
        public IEnumerable<Data.Models.ScheduledShift> Map(IEnumerable<ScheduledShift> source)
        {
            return source.Select(ss => new Data.Models.ScheduledShift
            {
                Id = ss.Id,
                Day = ss.Day,
                Start = ss.Start,
                End = ss.End,
                Employees = ss.Employees.Select(e => new Data.Models.Employee
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
                })
            });
        }
    }
}
