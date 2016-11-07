using System.Collections.Generic;
using System.Linq;
using Data.Models;
using DataTransferObjects.OpeningHours;

namespace API.Mapping
{
    public class OpeningHoursMapper : IOpeningHoursMapper
    {
        public IEnumerable<OpeningHoursDTO> MapToDto(IEnumerable<Shift> source)
        {
            var shifts = source as IList<Shift> ?? source.ToList();

            return shifts.Select(shift => new OpeningHoursDTO
            {
                Id = shift.Id,
                Start = shift.Start,
                End = shift.End,
                Employees = shift.Employees.Select(emp => new OpeningHourEmployeeDTO
                {
                    Id = emp.Id,
                    FirstName = emp.FirstName
                }),
                CheckedInEmployees = shift.CheckIns.Select(checkIn => new OpeningHourEmployeeDTO
                {
                    Id = checkIn.Employee.Id,
                    FirstName = checkIn.Employee.FirstName
                })
            });
        }
    }
}