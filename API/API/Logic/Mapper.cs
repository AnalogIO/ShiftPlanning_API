using API.Controllers;
using Data.Models;
using DataTransferObjects.Employee;
using DataTransferObjects.EmployeeTitles;
using DataTransferObjects.Schedule;
using DataTransferObjects.Shift;
using System.Collections.Generic;
using System.Web;

namespace API.Logic
{
    public static class Mapper
    {
        public static EmployeeDTO Map(Employee employee)
        {
            var url = HttpContext.Current.Request.Url;

            var portString = url.IsDefaultPort ? "" : $":{url.Port}";

            return new EmployeeDTO
            {
                Id = employee.Id,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Email = employee.Email,
                Active = employee.Active,
                EmployeeTitle = employee.EmployeeTitle?.Title,
                EmployeeTitleId = employee.EmployeeTitle?.Id,
                PhotoRef = $"{url.Scheme}://{url.Host}{portString}/shiftplanning/{PhotosController.RoutePrefix}/{employee.Photo.Id}/{employee.Organization.Id}" // ugly temp fix for /shiftplanning/
            };
        }

        public static IEnumerable<EmployeeDTO> Map(IEnumerable<Employee> employees)
        {
            foreach(Employee employee in employees)
            {
                yield return Map(employee);
            }
        }

        public static ScheduledShiftDTO Map(ScheduledShift scheduledShift)
        {
            return new ScheduledShiftDTO { Id = scheduledShift.Id, Day = scheduledShift.Day, Start = scheduledShift.Start, End = scheduledShift.End, Employees = Map(scheduledShift.Employees) };
        }

        public static IEnumerable<ScheduledShiftDTO> Map(IEnumerable<ScheduledShift> scheduledShifts)
        {
            foreach (ScheduledShift scheduledShift in scheduledShifts)
            {
                yield return Map(scheduledShift);
            }
        }

        public static ScheduleDTO Map(Schedule schedule)
        {
            return new ScheduleDTO { Id = schedule.Id, Name = schedule.Name, NumberOfWeeks = schedule.NumberOfWeeks, ScheduledShifts = Map(schedule.ScheduledShifts) };
        }

        public static IEnumerable<ScheduleDTO> Map(IEnumerable<Schedule> schedules)
        {
            foreach (Schedule schedule in schedules)
            {
                yield return Map(schedule);
            }
        }

        public static CheckInDTO Map(CheckIn checkIn)
        {
            return new CheckInDTO { Id = checkIn.Id, Time = checkIn.Time, Employee = Map(checkIn.Employee) };
        }

        public static IEnumerable<CheckInDTO> Map(IEnumerable<CheckIn> checkIns)
        {
            foreach (CheckIn checkIn in checkIns)
            {
                yield return Map(checkIn);
            }
        }

        public static ShiftDTO Map(Shift shift)
        {
            return new ShiftDTO { Id = shift.Id, Start = shift.Start, End = shift.End, CheckIns = Map(shift.CheckIns), Employees = Map(shift.Employees) };
        }

        public static IEnumerable<ShiftDTO> Map(IEnumerable<Shift> shifts)
        {
            foreach (Shift shift in shifts)
            {
                yield return Map(shift);
            }
        }

        public static EmployeeTitleDTO Map(EmployeeTitle employeeTitle)
        {
            return new EmployeeTitleDTO { Id = employeeTitle.Id, Title = employeeTitle.Title };
        }

        public static IEnumerable<EmployeeTitleDTO> Map(IEnumerable<EmployeeTitle> employeeTitles)
        {
            foreach (EmployeeTitle employeeTitle in employeeTitles)
            {
                yield return Map(employeeTitle);
            }
        }
    }
}