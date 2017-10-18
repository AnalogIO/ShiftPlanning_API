using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel;
using System.Linq;
using Data.Models;
using Data.Repositories;
using DataTransferObjects.Employee;
using DataTransferObjects.Schedule;
using DataTransferObjects.ScheduledShift;

namespace Data.Services
{
    public class ScheduleService : IScheduleService
    {
        private readonly IScheduleRepository _scheduleRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IShiftRepository _shiftRepository;

        public ScheduleService(IScheduleRepository scheduleRepository, IEmployeeRepository employeeRepository, IShiftRepository shiftRepository)
        {
            _scheduleRepository = scheduleRepository;
            _employeeRepository = employeeRepository;
            _shiftRepository = shiftRepository;
        }

        public ScheduledShift CreateScheduledShift(CreateScheduledShiftDTO scheduledShiftDto, Employee employee, int scheduleId)
        {
            var schedule = _scheduleRepository.Read(scheduleId, employee.Organization.Id);
            if (schedule == null) throw new ObjectNotFoundException("Could not find a schedule corresponding to the given id");
            var employees = _employeeRepository.ReadFromOrganization(employee.Organization.Id).Where(x => scheduledShiftDto.EmployeeIds.Contains(x.Id)).ToList();
            var scheduledShift = new ScheduledShift { Day = scheduledShiftDto.Day, Start = TimeSpan.Parse(scheduledShiftDto.Start), End = TimeSpan.Parse(scheduledShiftDto.End), Schedule = schedule, MaxOnShift = scheduledShiftDto.MaxOnShift, MinOnShift = scheduledShiftDto.MinOnShift };
            scheduledShift.EmployeeAssignments =
                employees.Select(
                    e => new EmployeeAssignment {Employee = e, ScheduledShift = scheduledShift, IsLocked = false}).ToList();
            schedule.ScheduledShifts.Add(scheduledShift);
            return _scheduleRepository.Update(schedule) > 0 ? scheduledShift : null;
        }

        public IEnumerable<ScheduledShift> CreateScheduledShifts(IEnumerable<CreateScheduledShiftDTO> scheduledShiftsDto, Employee employee, int scheduleId)
        {
            var schedule = _scheduleRepository.Read(scheduleId, employee.Organization.Id);
            if (schedule == null) throw new ObjectNotFoundException("Could not find a schedule corresponding to the given id");
            var scheduledShifts = new List<ScheduledShift>();
            foreach(CreateScheduledShiftDTO scheduledShiftDto in scheduledShiftsDto)
            {
                var employees = _employeeRepository.ReadFromOrganization(employee.Organization.Id).Where(x => scheduledShiftDto.EmployeeIds.Contains(x.Id)).ToList();
                var scheduledShift = new ScheduledShift { Day = scheduledShiftDto.Day, Start = TimeSpan.Parse(scheduledShiftDto.Start), End = TimeSpan.Parse(scheduledShiftDto.End), Schedule = schedule, MaxOnShift = scheduledShiftDto.MaxOnShift, MinOnShift = scheduledShiftDto.MinOnShift};
                scheduledShift.EmployeeAssignments =
                employees.Select(
                    e => new EmployeeAssignment { Employee = e, ScheduledShift = scheduledShift, IsLocked = false }).ToList();
                scheduledShifts.Add(scheduledShift);
                schedule.ScheduledShifts.Add(scheduledShift);
            }
            return _scheduleRepository.Update(schedule) > 0 ? scheduledShifts : null;
        }

        public ScheduledShift UpdateScheduledShift(int scheduledShiftId, int scheduleId, UpdateScheduledShiftDTO scheduledShiftDto, Employee employee)
        {
            var dbSchedule = _scheduleRepository.Read(scheduleId, employee.Organization.Id);

            var employees = _employeeRepository.ReadFromOrganization(employee.Organization.Id).Where(x => scheduledShiftDto.EmployeeIds.Contains(x.Id)).ToList();

            var dbScheduledShift = dbSchedule.ScheduledShifts.Single(s => s.Id == scheduledShiftId);

            foreach(var emp in dbScheduledShift.EmployeeAssignments.Select(e => e.Employee).ToList())
            {
                emp.EmployeeAssignments =
                    emp.EmployeeAssignments.Where(ea => ea.ScheduledShift.Id != dbScheduledShift.Id).ToList();
            }

            dbScheduledShift.Start = TimeSpan.Parse(scheduledShiftDto.Start);
            dbScheduledShift.End = TimeSpan.Parse(scheduledShiftDto.End);
            dbScheduledShift.MaxOnShift = scheduledShiftDto.MaxOnShift;
            dbScheduledShift.MinOnShift = scheduledShiftDto.MinOnShift;
            dbScheduledShift.Day = scheduledShiftDto.Day;
            dbScheduledShift.EmployeeAssignments =
                employees.Select(
                    e => new EmployeeAssignment { Employee = e, ScheduledShift = dbScheduledShift, IsLocked = scheduledShiftDto.LockedEmployeeIds.Any(lei => lei == e.Id) }).ToList();

            _scheduleRepository.Update(dbSchedule);

            return dbSchedule.ScheduledShifts.Single(s => s.Id == scheduledShiftId);
        }

        public Schedule CreateSchedule(CreateScheduleDTO scheduleDto, Employee employee)
        {
            var schedule = new Schedule { Name = scheduleDto.Name, NumberOfWeeks = scheduleDto.NumberOfWeeks, Organization = employee.Organization, ScheduledShifts = new List<ScheduledShift>(), Shifts = new List<Shift>() };
            return _scheduleRepository.Create(schedule);
        }

        public Schedule GetSchedule(int scheduleId, Employee employee)
        {
            return _scheduleRepository.Read(scheduleId, employee.Organization.Id);
        }

        public IEnumerable<Schedule> GetSchedules(Employee employee)
        {
            return _scheduleRepository.ReadFromOrganization(employee.Organization.Id);
        }

        public void DeleteSchedule(int scheduleId, Employee employee)
        {
            _scheduleRepository.Delete(scheduleId, employee.Organization.Id);
        }

        public Schedule UpdateSchedule(int scheduleId, UpdateScheduleDTO scheduleDto, Employee employee)
        {
            var schedule = _scheduleRepository.Read(scheduleId, employee.Organization.Id);
            if (schedule == null) throw new ObjectNotFoundException("Could not find a schedule corresponding to the given id");

            schedule.Name = scheduleDto.Name;
            schedule.NumberOfWeeks = scheduleDto.NumberOfWeeks;

            _scheduleRepository.Update(schedule);

            return schedule;
        }

        public Schedule UpdateSchedule(Schedule schedule, Employee employee)
        {
            _scheduleRepository.Update(schedule);
            return schedule;
        }

        public IEnumerable<Shift> RolloutSchedule(int scheduleId, RolloutScheduleDTO rolloutDto, Employee employee)
        {
            if (rolloutDto.StartFromScheduledWeek == 0) rolloutDto.StartFromScheduledWeek = 1;

            var from = DateTimeOffset.Parse(rolloutDto.From).LocalDateTime;
            var to = DateTimeOffset.Parse(rolloutDto.To).LocalDateTime;

            if(from >= to) throw new BadRequestException("'From' date should be before 'To' date");

            var currentDate = from;
            var currentDay = (int)from.DayOfWeek + (rolloutDto.StartFromScheduledWeek - 1) * 7;

            var schedule = _scheduleRepository.Read(scheduleId, employee.Organization.Id);

            if (schedule == null) throw new ObjectNotFoundException("Could not find a schedule corresponding to the given id");

            var shifts = new List<Shift>();
            for(var i = 0; i <= ((to - from).TotalDays / (7*schedule.NumberOfWeeks)); i++)
            {
                foreach (ScheduledShift scheduledShift in schedule.ScheduledShifts.OrderBy(x => x.Day))
                {
                    if (currentDay < scheduledShift.Day)
                    {
                        currentDate = currentDate.AddDays(scheduledShift.Day - currentDay);
                        currentDay = scheduledShift.Day;
                    }

                    if (currentDate > to) break;

                    if (scheduledShift.Day == currentDay)
                    {
                        var shift = new Shift
                        {
                            Start = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, scheduledShift.Start.Hours, scheduledShift.Start.Minutes, scheduledShift.Start.Seconds),
                            End = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, scheduledShift.End.Hours, scheduledShift.End.Minutes, scheduledShift.End.Seconds),
                            CheckIns = new List<CheckIn>(),
                            Employees = scheduledShift.EmployeeAssignments.Select(ea => ea.Employee).ToList(),
                            Organization = employee.Organization,
                            Schedule = schedule
                        };
                        shifts.Add(shift);
                    }
                }
                currentDate = currentDate.AddDays((schedule.NumberOfWeeks * 7) - currentDay);
                currentDay = 0;
            }
            var now = DateTime.Now;
            var shiftsToRemove = schedule.Shifts.Where(s => s.Start > from && s.End > now && !s.CheckIns.Any()); // delete all previous rolled out shifts that will take place in the future
            var shiftDaysWithCheckIn = schedule.Shifts.Where(s => s.CheckIns.Any()).Select(ss => ss.Start.DayOfYear); // get which days are protected because of existing check ins
            shifts = shifts.Where(s => !shiftDaysWithCheckIn.Contains(s.Start.DayOfYear)).ToList(); // remove generated shifts that interferes with protected days
            _shiftRepository.Delete(shiftsToRemove);
            _scheduleRepository.Update(schedule);
            return _shiftRepository.Create(shifts);
        }

        public void DeleteScheduledShift(int scheduleId, int scheduledShiftId, Employee employee)
        {
            _scheduleRepository.DeleteScheduledShift(scheduleId, scheduledShiftId, employee.Organization.Id);
        }

        public IEnumerable<ScheduledShift> GetScheduledShifts(IEnumerable<int> scheduledShiftIds)
        {
            return _scheduleRepository.GetScheduledShifts(scheduledShiftIds);
        }

        public IEnumerable<Preference> CreateOrUpdatePreferences(Employee employee, int scheduleId, IEnumerable<PreferenceDTO> preferences)
        {
            var scheduledShifts = _scheduleRepository.Read(scheduleId, employee.Organization.Id).ScheduledShifts.Where(s => preferences.Select(p => p.ScheduledShiftId).Contains(s.Id)).ToList();
            var prefsToRemove = employee.Preferences.Where(p => p.ScheduledShift.Schedule.Id == scheduleId).ToList();

            _scheduleRepository.DeletePreferences(prefsToRemove);

            foreach (var p in preferences)
            {
                var newPref = new Preference
                {
                    Employee = employee,
                    Priority = p.Priority,
                    ScheduledShift = scheduledShifts.Single(s => s.Id == p.ScheduledShiftId)
                };
                employee.Preferences.Add(newPref);   
            }
            _employeeRepository.Update(employee);
            return employee.Preferences;
        }

    }
}