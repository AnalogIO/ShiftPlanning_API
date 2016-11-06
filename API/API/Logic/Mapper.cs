using Data.Models;
using DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Logic
{
    public static class Mapper
    {
        public static EmployeeDTO Map(Employee employee)
        {
            return new EmployeeDTO { Id = employee.Id, FirstName = employee.FirstName, LastName = employee.LastName, EmployeeTitle = employee.EmployeeTitle?.Title, EmployeeTitleId = employee.EmployeeTitle?.Id };
        }

        public static IEnumerable<EmployeeDTO> Map(IEnumerable<Employee> employees)
        {
            var employeeDtos = new List<EmployeeDTO>();
            foreach(Employee employee in employees)
            {
                employeeDtos.Add(Map(employee));
            }
            return employeeDtos;
        }

        public static ScheduledShiftDTO Map(ScheduledShift scheduledShift)
        {
            return new ScheduledShiftDTO { Id = scheduledShift.Id, Day = scheduledShift.Day, Start = scheduledShift.Start, End = scheduledShift.End, Employees = Map(scheduledShift.Employees) };
        }

        public static IEnumerable<ScheduledShiftDTO> Map(IEnumerable<ScheduledShift> scheduledShifts)
        {
            var scheduledShiftDtos = new List<ScheduledShiftDTO>();
            foreach (ScheduledShift scheduledShift in scheduledShifts)
            {
                scheduledShiftDtos.Add(Map(scheduledShift));
            }
            return scheduledShiftDtos;
        }

        public static ScheduleDTO Map(Schedule schedule)
        {
            return new ScheduleDTO { Id = schedule.Id, Name = schedule.Name, NumberOfWeeks = schedule.NumberOfWeeks, SchedulesShifts = Map(schedule.ScheduledShifts) };
        }

        public static IEnumerable<ScheduleDTO> Map(IEnumerable<Schedule> schedules)
        {
            var scheduleDtos = new List<ScheduleDTO>();
            foreach (Schedule schedule in schedules)
            {
                scheduleDtos.Add(Map(schedule));
            }
            return scheduleDtos;
        }
    }
}