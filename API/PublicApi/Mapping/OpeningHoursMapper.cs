using System;
using System.Collections.Generic;
using System.Linq;
using Data.Models;
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
                Open = shift.Start.ToUniversalTime().ToLocalTime(), // hack to add the timezone (old tamigo format)
                Close = shift.End.ToUniversalTime().ToLocalTime(), // hack to add the timezone (old tamigo format)
                Employees = shift.Employees.Select(emp => emp.FirstName),
                CheckedInEmployees = shift.CheckIns.Select(checkIn => new OpeningHourEmployeeDTO
                {
                    Id = checkIn.Employee.Id,
                    FirstName = checkIn.Employee.FirstName
                })
            });
        }

        public IntervalOpeningHoursDTO MapToIntervalDto(ICollection<Shift> shifts, int interval)
        {
            var openingHoursDto = new IntervalOpeningHoursDTO
            {
                IntervalMinutes = interval,
                Shifts = new SortedDictionary<string, ICollection<IntervalOpeningHourDTO>>(),
                EndHour = 16,
                StartHour = 8
            };

            if (!shifts.Any()) return openingHoursDto; // if no shifts are available then return the opening hours dto with an empty dictionary for shifts.

            var startDate = shifts.OrderBy(x => x.Start).First();
            var endDate = shifts.OrderBy(x => x.End).Last();

            var start = shifts.Min(s => s.Start.Hour);
            var end = shifts.Max(s => s.End.Hour);

            for (var i = startDate.Start.DayOfYear; i <= endDate.Start.DayOfYear; i++)
            {
                var currentDate = new DateTime(DateTime.Now.Year, 1, 1, start, 0, 0).AddDays(i - 1);
                var currentDateString = $"{currentDate:yyyy-MM-dd}";
                if (currentDate.DayOfWeek == DayOfWeek.Saturday || currentDate.DayOfWeek == DayOfWeek.Sunday) continue;
                while (currentDate.Hour < end)
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