using System;
using System.Collections.Generic;
using System.Linq;
using Data.Models;
using DataTransferObjects.Public.Employees;
using DataTransferObjects.Public.OpeningHours;

namespace PublicApi.Mapping
{
    /// <inheritdoc cref="IOpeningHoursMapper"/>
    public class OpeningHoursMapper : IOpeningHoursMapper
    {

        /// <inheritdoc cref="MapToDto"/>
        public IEnumerable<OpeningHoursDTO> MapToDto(IEnumerable<Shift> source)
        {
            var shifts = source as IList<Shift> ?? source.ToList();

            return shifts.Select(shift => new OpeningHoursDTO
            {
                Id = shift.Id,
                Start = shift.Start, // hack to add the timezone (old tamigo format)
                End = shift.End, // hack to add the timezone (old tamigo format)
                Employees = shift.Employees.Select(emp => new EmployeeDTO
                {
                    FirstName = emp.FirstName,
                    LastName = emp.LastName
                }),
                CheckedInEmployees = shift.CheckIns.Select(checkIn => new OpeningHourEmployeeDTO
                {
                    Id = checkIn.Employee.Id,
                    FirstName = checkIn.Employee.FirstName
                })
            });
        }

        public IntervalOpeningHoursDTO MapToIntervalDto(ICollection<Shift> shifts, int interval)
        {
            var now = DateTime.Now;
            var ongoing = shifts.Where(shift => shift.Start <= now && now <= shift.End);
            var currentlyCheckedIn = ongoing.SelectMany(s => s.CheckIns);

            var openingHoursDto = new IntervalOpeningHoursDTO
            {
                IntervalMinutes = interval,
                Shifts = new SortedDictionary<string, ICollection<IntervalOpeningHourDTO>>(),
                EndHour = 16,
                StartHour = 8,
                CheckedInEmployees = currentlyCheckedIn.Select(checkIn => new OpeningHourEmployeeDTO
                {
                    Id = checkIn.Employee.Id,
                    FirstName = checkIn.Employee.FirstName
                })
            };

            if (!shifts.Any()) return openingHoursDto; // if no shifts are available then return the opening hours dto with an empty dictionary for shifts.

            var startDate = shifts.OrderBy(x => x.Start).First();
            var endDate = shifts.OrderBy(x => x.End).Last();

            var earliestShift = shifts.OrderBy(s => s.Start.Hour).FirstOrDefault();
            var latestShift = shifts.OrderByDescending(s => s.End.Hour).FirstOrDefault();
            var start = earliestShift.Start.Hour;
            var end = latestShift.End.Hour;

            if (latestShift.End.Minute > 0) end++; // if latest shift is 17:30 we round up to 18

            if (start < openingHoursDto.StartHour) openingHoursDto.StartHour = start;
            else start = openingHoursDto.StartHour;

            if (end > openingHoursDto.EndHour) openingHoursDto.EndHour = end;
            else end = openingHoursDto.EndHour;

            for (var i = startDate.Start.DayOfYear; i <= endDate.Start.DayOfYear; i++)
            {
                var currentDate = new DateTime(DateTime.Now.Year, 1, 1, start, 0, 0).AddDays(i - 1);
                var currentDateString = $"{currentDate:yyyy-MM-dd}";
                if (currentDate.DayOfWeek == DayOfWeek.Saturday || currentDate.DayOfWeek == DayOfWeek.Sunday)
                {
                    if(!shifts.Any(s => s.Start.DayOfYear == currentDate.DayOfYear || s.End.DayOfYear == currentDate.DayOfYear)) continue;
                }
                //if (currentDate.DayOfWeek == DayOfWeek.Saturday || currentDate.DayOfWeek == DayOfWeek.Sunday) continue;
                while (currentDate.Hour < end && currentDate.DayOfYear == i)
                {
                    var openingHourShift = new IntervalOpeningHourDTO { ShiftStart = currentDate, Employees = new List<OpeningHourEmployeeDTO>() };

                    foreach (var shift in shifts.Where(x => x.Start <= currentDate && x.End >= currentDate.AddMinutes(interval)).ToList())
                    {
                        openingHourShift.Open = shift.Employees.Any();
                        openingHourShift.Employees = shift.Employees.Select(emp => new OpeningHourEmployeeDTO
                        {
                            FirstName = emp.FirstName,
                            Id = emp.Id
                        });
                    }
                    if (!openingHoursDto.Shifts.ContainsKey(currentDateString))
                    {
                        openingHoursDto.Shifts.Add(currentDateString, new List<IntervalOpeningHourDTO>());
                    }
                    openingHoursDto.Shifts[currentDateString].Add(openingHourShift);
                    currentDate = currentDate.AddMinutes(interval);
                }
            }

            return openingHoursDto;
        }
    }
}