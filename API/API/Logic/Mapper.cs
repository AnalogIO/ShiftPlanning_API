using System;
using API.Controllers;
using Data.Models;
using DataTransferObjects.Employee;
using DataTransferObjects.EmployeeTitles;
using DataTransferObjects.Schedule;
using DataTransferObjects.Shift;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataTransferObjects.General;
using DataTransferObjects.Friendship;

namespace API.Logic
{
    public static class Mapper
    {
        public static GeneralMessage Map(string message)
        {
            return new GeneralMessage { Message = message };
        }

        public static EmployeeDTO Map(Employee employee)
        {
            var url = HttpContext.Current.Request.Url.AbsoluteUri;

            var routeBase = url.Substring(0, url.IndexOf("/api/", StringComparison.Ordinal));

            return new EmployeeDTO
            {
                Id = employee.Id,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Email = employee.Email,
                Active = employee.Active,
                EmployeeTitle = employee.EmployeeTitle?.Title,
                EmployeeTitleId = employee.EmployeeTitle?.Id,
                PhotoRef = $"{routeBase}/{PhotosController.RoutePrefix}/{employee.Photo?.Id}/{employee.Organization.Id}",
                CheckInCount = employee.CheckIns?.Count,
                Roles = employee.Roles.Select(r => r.Name).ToArray(),
                WantShifts = employee.WantShifts,
                FriendshipIds = employee.Friendships.Select(f => f.Friend_Id).ToArray()
            };
        }

        public static IEnumerable<EmployeeDTO> Map(IEnumerable<Employee> employees)
        {
            return employees.Select(Map);
        }

        public static ScheduledShiftDTO Map(ScheduledShift scheduledShift)
        {
            return new ScheduledShiftDTO { Id = scheduledShift.Id, Day = scheduledShift.Day, Start = scheduledShift.Start.ToString(@"hh\:mm"), End = scheduledShift.End.ToString(@"hh\:mm"), MaxOnShift = scheduledShift.MaxOnShift, MinOnShift = scheduledShift.MinOnShift, Employees = Map(scheduledShift.EmployeeAssignments.Select(ea => ea.Employee)), LockedEmployeeIds = scheduledShift.EmployeeAssignments.Where(ea => ea.IsLocked).Select(ea => ea.Employee.Id).ToArray()};
        }

        public static IEnumerable<ScheduledShiftDTO> Map(IEnumerable<ScheduledShift> scheduledShifts)
        {
            return scheduledShifts.Select(Map);
        }

        public static ScheduledShiftDTOSimple MapSimple(ScheduledShift scheduledShift)
        {
            return new ScheduledShiftDTOSimple { Id = scheduledShift.Id, Day = scheduledShift.Day, Start = scheduledShift.Start.ToString(@"hh\:mm"), End = scheduledShift.End.ToString(@"hh\:mm") };

        }

        public static IEnumerable<ScheduledShiftDTOSimple> MapSimple(IEnumerable<ScheduledShift> scheduledShifts)
        {
            return scheduledShifts.Select(MapSimple);
        }

        public static ScheduleDTO Map(Schedule schedule)
        {
            return new ScheduleDTO { Id = schedule.Id, Name = schedule.Name, NumberOfWeeks = schedule.NumberOfWeeks, ScheduledShifts = Map(schedule.ScheduledShifts) };
        }

        public static IEnumerable<ScheduleDTO> Map(IEnumerable<Schedule> schedules)
        {
            return schedules.Select(Map);
        }

        public static ScheduleDTOSimple MapSimple(Schedule schedule)
        {
            return new ScheduleDTOSimple { Id = schedule.Id, Name = schedule.Name, NumberOfWeeks = schedule.NumberOfWeeks, ScheduledShifts = MapSimple(schedule.ScheduledShifts) };
        }

        public static IEnumerable<ScheduleDTOSimple> MapSimple(IEnumerable<Schedule> schedules)
        {
            return schedules.Select(MapSimple);
        }

        public static CheckInDTO Map(CheckIn checkIn)
        {
            return new CheckInDTO { Id = checkIn.Id, Time = checkIn.Time, Employee = Map(checkIn.Employee) };
        }

        public static IEnumerable<CheckInDTO> Map(IEnumerable<CheckIn> checkIns)
        {
            return checkIns.Select(Map);
        }

        public static ShiftDTO Map(Shift shift)
        {
            return new ShiftDTO { Id = shift.Id, Start = shift.Start, End = shift.End, CheckIns = Map(shift.CheckIns), Employees = Map(shift.Employees) };
        }

        public static IEnumerable<ShiftDTO> Map(IEnumerable<Shift> shifts)
        {
            return shifts.Select(Map);
        }

        public static EmployeeTitleDTO Map(EmployeeTitle employeeTitle)
        {
            return new EmployeeTitleDTO { Id = employeeTitle.Id, Title = employeeTitle.Title };
        }

        public static IEnumerable<EmployeeTitleDTO> Map(IEnumerable<EmployeeTitle> employeeTitles)
        {
            return employeeTitles.Select(Map);
        }

        public static PreferenceDTO Map(Preference preference)
        {
            return new PreferenceDTO {Priority = preference.Priority, ScheduledShiftId = preference.ScheduledShift.Id};
        }

        public static IEnumerable<PreferenceDTO> Map(IEnumerable<Preference> preferences)
        {
            return preferences.Select(Map);
        }

        public static EmployeeDTOSimple MapSimple(Employee employee)
        {
            return new EmployeeDTOSimple { Id = employee.Id, FirstName = employee.FirstName, LastName = employee.LastName };
        }

        public static IEnumerable<EmployeeDTOSimple> MapSimple(IEnumerable<Employee> employees)
        {
            return employees.Select(MapSimple);
        }

        public static IEnumerable<FriendshipDTO> Map(IEnumerable<Friendship> friendships)
        {
            return friendships.Select(Map);
        }

        public static FriendshipDTO Map(Friendship friendship)
        {
            return new FriendshipDTO { Id = friendship.Id, EmployeeId = friendship.Employee_Id, FriendId = friendship.Friend_Id };
        }

        public static FindOptimalScheduleDTO MapToFindOptimalScheduleDto(Schedule schedule)
        {
            var employees = schedule.ScheduledShifts.SelectMany(ss => ss.Preferences.Select(p => p.Employee)).Distinct();
            var preferences = new List<FindOptimalSchedulePreferencesDTO>();
            var lockedIds = new List<int>();
            foreach (var employee in employees)
            {
                var lockedTo = employee.EmployeeAssignments.Where(ea => ea.IsLocked).Select(ea => ea.ScheduledShift.Id).ToList();
                lockedIds.AddRange(lockedTo);
                if(lockedTo.Count > 0) continue;

                var prefs =
                    employee.Preferences.Where(p => p.ScheduledShift.Schedule.Id == schedule.Id)
                        .Select(
                            p =>
                                new FindOptimalSchedulePreferencesDTO.FindOptimalSchedulePreference
                                {
                                    ScheduledShiftId = p.ScheduledShift.Id,
                                    Priority = p.Priority
                                });
                var baristaPrefs = new FindOptimalSchedulePreferencesDTO()
                {
                    BaristaId = employee.Id,
                    Preferences = prefs,
                    Friendships = employee.Friendships.Select(f => f.Friend_Id).ToArray(),
                    WantShifts = employee.WantShifts
                };
                preferences.Add(baristaPrefs);
            }
            var dto = new FindOptimalScheduleDTO
            {
                Preferences = preferences,
                Shifts =
                    schedule.ScheduledShifts.Select(
                        ss => new FindOptimalScheduleShiftDTO {Id = ss.Id, MaxOnShift = ss.MaxOnShift, MinOnShift = ss.MinOnShift})
            };

            // subtract lockedIds from maxOnShift and minOnShift
            foreach (var t in lockedIds)
            {
                var findOptimalScheduleShiftDto = dto.Shifts.FirstOrDefault(s => s.Id == t);
                if(findOptimalScheduleShiftDto == null) continue;
                findOptimalScheduleShiftDto.MaxOnShift--;
                findOptimalScheduleShiftDto.MinOnShift--;
            }
            return dto;
        }
    }
}