using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel;
using System.Linq;
using Data.Models;
using Data.Repositories;
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

        public ScheduledShift CreateScheduledShift(CreateScheduledShiftDTO scheduledShiftDto, Manager manager, int scheduleId)
        {
            var schedule = _scheduleRepository.Read(scheduleId, manager.Organization.Id);
            if (schedule == null) throw new ObjectNotFoundException("Could not find a schedule corresponding to the given id");
            var employees = _employeeRepository.ReadFromOrganization(manager.Organization.Id).Where(x => scheduledShiftDto.EmployeeIds.Contains(x.Id)).ToList();
            var scheduledShift = new ScheduledShift { Day = scheduledShiftDto.Day, Start = TimeSpan.Parse(scheduledShiftDto.Start), End = TimeSpan.Parse(scheduledShiftDto.End), Schedule = schedule, Employees = employees };
            schedule.ScheduledShifts.Add(scheduledShift);
            return _scheduleRepository.Update(schedule) > 0 ? scheduledShift : null;
        }

        public IEnumerable<ScheduledShift> CreateScheduledShifts(IEnumerable<CreateScheduledShiftDTO> scheduledShiftsDto, Manager manager, int scheduleId)
        {
            var schedule = _scheduleRepository.Read(scheduleId, manager.Organization.Id);
            if (schedule == null) throw new ObjectNotFoundException("Could not find a schedule corresponding to the given id");
            var scheduledShifts = new List<ScheduledShift>();
            foreach(CreateScheduledShiftDTO scheduledShiftDto in scheduledShiftsDto)
            {
                var employees = _employeeRepository.ReadFromOrganization(manager.Organization.Id).Where(x => scheduledShiftDto.EmployeeIds.Contains(x.Id)).ToList();
                var scheduledShift = new ScheduledShift { Day = scheduledShiftDto.Day, Start = TimeSpan.Parse(scheduledShiftDto.Start), End = TimeSpan.Parse(scheduledShiftDto.End), Schedule = schedule, Employees = employees };
                scheduledShifts.Add(scheduledShift);
                schedule.ScheduledShifts.Add(scheduledShift);
            }
            return _scheduleRepository.Update(schedule) > 0 ? scheduledShifts : null;
        }

        public ScheduledShift UpdateScheduledShift(int scheduledShiftId, int scheduleId, UpdateScheduledShiftDTO scheduledShiftDto, Manager manager)
        {
            var dbSchedule = _scheduleRepository.Read(scheduleId, manager.Organization.Id);

            var employees = _employeeRepository.ReadFromOrganization(manager.Organization.Id).Where(x => scheduledShiftDto.EmployeeIds.Contains(x.Id)).ToList();

            dbSchedule.ScheduledShifts.Single(s => s.Id == scheduledShiftId).Employees.Clear();

            dbSchedule.ScheduledShifts.Single(s => s.Id == scheduledShiftId).Start = TimeSpan.Parse(scheduledShiftDto.Start);
            dbSchedule.ScheduledShifts.Single(s => s.Id == scheduledShiftId).End = TimeSpan.Parse(scheduledShiftDto.End);
            dbSchedule.ScheduledShifts.Single(s => s.Id == scheduledShiftId).Day = scheduledShiftDto.Day;
            dbSchedule.ScheduledShifts.Single(s => s.Id == scheduledShiftId).Employees = employees;

            _scheduleRepository.Update(dbSchedule);

            return dbSchedule.ScheduledShifts.Single(s => s.Id == scheduledShiftId);
        }

        public Schedule CreateSchedule(CreateScheduleDTO scheduleDto, Manager manager)
        {
            var schedule = new Schedule { Name = scheduleDto.Name, NumberOfWeeks = scheduleDto.NumberOfWeeks, Organization = manager.Organization, ScheduledShifts = new List<ScheduledShift>(), Shifts = new List<Shift>() };
            return _scheduleRepository.Create(schedule);
        }

        public Schedule GetSchedule(int scheduleId, Manager manager)
        {
            return _scheduleRepository.Read(scheduleId, manager.Organization.Id);
        }

        public IEnumerable<Schedule> GetSchedules(Manager manager)
        {
            return _scheduleRepository.ReadFromOrganization(manager.Organization.Id);
        }

        public void DeleteSchedule(int scheduleId, Manager manager)
        {
            _scheduleRepository.Delete(scheduleId, manager.Organization.Id);
        }

        public Schedule UpdateSchedule(int scheduleId, UpdateScheduleDTO scheduleDto, Manager manager)
        {
            var schedule = _scheduleRepository.Read(scheduleId, manager.Organization.Id);
            if (schedule == null) throw new ObjectNotFoundException("Could not find a schedule corresponding to the given id");

            schedule.Name = scheduleDto.Name;
            schedule.NumberOfWeeks = scheduleDto.NumberOfWeeks;

            _scheduleRepository.Update(schedule);

            return schedule;
        }

        public IEnumerable<Shift> RolloutSchedule(int scheduleId, RolloutScheduleDTO rolloutDto, Manager manager)
        {
            if (rolloutDto.StartFromScheduledWeek == 0) rolloutDto.StartFromScheduledWeek = 1;

            var from = DateTimeOffset.Parse(rolloutDto.From).LocalDateTime;
            var to = DateTimeOffset.Parse(rolloutDto.To).LocalDateTime;

            if(from >= to) throw new BadRequestException("'From' date should be before 'To' date");

            var currentDate = from;
            var currentDay = (int)from.DayOfWeek + (rolloutDto.StartFromScheduledWeek - 1) * 7;

            var schedule = _scheduleRepository.Read(scheduleId, manager.Organization.Id);

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
                            Employees = scheduledShift.Employees,
                            Organization = manager.Organization,
                            Schedule = schedule
                        };
                        shifts.Add(shift);
                    }
                }
                currentDate = currentDate.AddDays((schedule.NumberOfWeeks * 7) - currentDay);
                currentDay = 0;
            }
            var shiftsToRemove = schedule.Shifts.Where(s => s.Start > from && !s.CheckIns.Any());
            var shiftDaysWithCheckIn = schedule.Shifts.Where(s => s.CheckIns.Any()).Select(ss => ss.Start.DayOfYear); // get which days are protected because of existing check ins
            shifts = shifts.Where(s => !shiftDaysWithCheckIn.Contains(s.Start.DayOfYear)).ToList(); // remove generated shifts that interferes with protected days
            _shiftRepository.Delete(shiftsToRemove);
            _scheduleRepository.Update(schedule);
            return _shiftRepository.Create(shifts);
        }

        public void DeleteScheduledShift(int scheduleId, int scheduledShiftId, Manager manager)
        {
            _scheduleRepository.DeleteScheduledShift(scheduleId, scheduledShiftId, manager.Organization.Id);
        }

    }
}