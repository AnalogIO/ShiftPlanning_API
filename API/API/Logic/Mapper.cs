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

        public static CheckInDTO Map(CheckIn checkIn)
        {
            return new CheckInDTO { Id = checkIn.Id, Time = checkIn.Time, Employee = Map(checkIn.Employee) };
        }

        public static IEnumerable<CheckInDTO> Map(IEnumerable<CheckIn> checkIns)
        {
            var checkInDtos = new List<CheckInDTO>();
            foreach (CheckIn checkIn in checkIns)
            {
                checkInDtos.Add(Map(checkIn));
            }
            return checkInDtos;
        }

        public static ShiftDTO Map(Shift shift)
        {
            return new ShiftDTO { Id = shift.Id, Start = shift.Start, End = shift.End, CheckIns = Map(shift.CheckIns), Employees = Map(shift.Employees) };
        }

        public static IEnumerable<ShiftDTO> Map(IEnumerable<Shift> shifts)
        {
            var shiftDtos = new List<ShiftDTO>();
            foreach (Shift shift in shifts)
            {
                shiftDtos.Add(Map(shift));
            }
            return shiftDtos;
        }

        public static EmployeeTitleDTO Map(EmployeeTitle employeeTitle)
        {
            return new EmployeeTitleDTO { Id = employeeTitle.Id, Title = employeeTitle.Title };
        }

        public static IEnumerable<EmployeeTitleDTO> Map(IEnumerable<EmployeeTitle> employeeTitles)
        {
            var employeeTitleDtos = new List<EmployeeTitleDTO>();
            foreach (EmployeeTitle employeeTitle in employeeTitles)
            {
                employeeTitleDtos.Add(Map(employeeTitle));
            }
            return employeeTitleDtos;
        }
    }
}