using Data.Models;
using Data.Repositories;
using DataTransferObjects;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace API.Logic
{
    public class ScheduleService
    {
        private readonly IScheduleRepository _scheduleRepository;
        private readonly IInstitutionRepository _institutionRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IShiftRepository _shiftRepository;

        public ScheduleService(IScheduleRepository scheduleRepository, IInstitutionRepository institutionRepository, IEmployeeRepository employeeRepository, IShiftRepository shiftRepository)
        {
            _scheduleRepository = scheduleRepository;
            _institutionRepository = institutionRepository;
            _employeeRepository = employeeRepository;
            _shiftRepository = shiftRepository;
        }

        public ScheduledShift CreateScheduledShift(CreateScheduledShiftDTO scheduledShiftDto, Manager manager, int scheduleId)
        {
            var schedule = _scheduleRepository.Read(scheduleId, manager.Institution.Id);
            if (schedule == null) return null;
            var employees = _employeeRepository.ReadFromInstitution(manager.Institution.Id).Where(x => scheduledShiftDto.EmployeeIds.Contains(x.Id)).ToList();
            var scheduledShift = new ScheduledShift { Day = scheduledShiftDto.Day, Start = TimeSpan.Parse(scheduledShiftDto.Start), End = TimeSpan.Parse(scheduledShiftDto.End), Schedule = schedule, Employees = employees };
            schedule.ScheduledShifts.Add(scheduledShift);
            return _scheduleRepository.Update(schedule) > 0 ? scheduledShift : null;
        }

        public IEnumerable<ScheduledShift> CreateScheduledShifts(IEnumerable<CreateScheduledShiftDTO> scheduledShiftsDto, Manager manager, int scheduleId)
        {
            var schedule = _scheduleRepository.Read(scheduleId, manager.Institution.Id);
            if (schedule == null) return null;
            var scheduledShifts = new List<ScheduledShift>();
            foreach(CreateScheduledShiftDTO scheduledShiftDto in scheduledShiftsDto)
            {
                var employees = _employeeRepository.ReadFromInstitution(manager.Institution.Id).Where(x => scheduledShiftDto.EmployeeIds.Contains(x.Id)).ToList();
                var scheduledShift = new ScheduledShift { Day = scheduledShiftDto.Day, Start = TimeSpan.Parse(scheduledShiftDto.Start), End = TimeSpan.Parse(scheduledShiftDto.End), Schedule = schedule, Employees = employees };
                scheduledShifts.Add(scheduledShift);
                schedule.ScheduledShifts.Add(scheduledShift);
            }
            return _scheduleRepository.Update(schedule) > 0 ? scheduledShifts : null;
        }

        public ScheduledShift UpdateScheduledShift(UpdateScheduledShiftDTO scheduledShiftDto, Institution institution, int scheduleId)
        {
            var dbSchedule = _scheduleRepository.Read(scheduleId, institution.Id);

            var employees = _employeeRepository.ReadFromInstitution(institution.Id).Where(x => scheduledShiftDto.EmployeeIds.Contains(x.Id)).ToList();

            dbSchedule.ScheduledShifts.Single(s => s.Id == scheduledShiftDto.Id).Employees.Clear();

            dbSchedule.ScheduledShifts.Single(s => s.Id == scheduledShiftDto.Id).Start = TimeSpan.Parse(scheduledShiftDto.Start);
            dbSchedule.ScheduledShifts.Single(s => s.Id == scheduledShiftDto.Id).End = TimeSpan.Parse(scheduledShiftDto.End);
            dbSchedule.ScheduledShifts.Single(s => s.Id == scheduledShiftDto.Id).Day = scheduledShiftDto.Day;
            dbSchedule.ScheduledShifts.Single(s => s.Id == scheduledShiftDto.Id).Employees = employees;

            _scheduleRepository.Update(dbSchedule);

            return dbSchedule.ScheduledShifts.Single(s => s.Id == scheduledShiftDto.Id);
        }

        public Schedule CreateSchedule(CreateScheduleDTO scheduleDto, Manager manager)
        {
            var schedule = new Schedule { Name = scheduleDto.Name, NumberOfWeeks = scheduleDto.NumberOfWeeks, Institution = manager.Institution, ScheduledShifts = new List<ScheduledShift>(), Shifts = new List<Shift>() };
            return _scheduleRepository.Create(schedule);
        }

        public Schedule GetSchedule(int scheduleId, Manager manager)
        {
            return _scheduleRepository.Read(scheduleId, manager.Institution.Id);
        }

        public IEnumerable<Schedule> GetSchedules(Manager manager)
        {
            return _scheduleRepository.ReadFromInstitution(manager.Institution.Id);
        }

        public void DeleteSchedule(int scheduleId, Manager manager)
        {
            _scheduleRepository.Delete(scheduleId, manager.Institution.Id);
        }

        public IEnumerable<Shift> RolloutSchedule(int scheduleId, string fromDate, string toDate, Manager manager)
        {
            var from = DateTime.Parse(fromDate);
            var to = DateTime.Parse(toDate);

            var currentDate = from;
            var currentDay = (int)from.DayOfWeek;

            var schedule = _scheduleRepository.Read(scheduleId, manager.Institution.Id);

            if (schedule == null) return null;

            var shifts = new List<Shift>();
            for(int i = 0; i <= ((to - from).TotalDays / (7*schedule.NumberOfWeeks)); i++)
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
                            Institution = manager.Institution,
                            Schedule = schedule
                        };
                        shifts.Add(shift);
                    }
                }
                currentDate = currentDate.AddDays((schedule.NumberOfWeeks * 7) - currentDay);
                currentDay = 0;
            }
            var shiftsToRemove = schedule.Shifts.Where(x => x.Start > from);
            _shiftRepository.Delete(shiftsToRemove);
            _scheduleRepository.Update(schedule);
            return _shiftRepository.Create(shifts);
        }
    }
}