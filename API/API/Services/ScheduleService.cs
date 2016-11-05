using Data.Models;
using Data.Repositories;
using DataTransferObjects;
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

        public ScheduleService(IScheduleRepository scheduleRepository, IInstitutionRepository institutionRepository, IEmployeeRepository employeeRepository)
        {
            _scheduleRepository = scheduleRepository;
            _institutionRepository = institutionRepository;
            _employeeRepository = employeeRepository;
        }

        public ScheduledShift CreateScheduledShift(CreateScheduledShiftDTO scheduledShiftDto, Institution institution, int scheduleId)
        {
            var schedule = _scheduleRepository.Read(scheduleId, institution.Id);
            if (schedule == null) return null;
            var employees = _employeeRepository.ReadFromInstitution(institution.Id).Where(x => scheduledShiftDto.EmployeeIds.Contains(x.Id)).ToList();
            var scheduledShift = new ScheduledShift { Day = scheduledShiftDto.Day, Start = TimeSpan.Parse(scheduledShiftDto.Start), End = TimeSpan.Parse(scheduledShiftDto.End), Schedule = schedule, Employees = employees };
            schedule.Shifts.Add(scheduledShift);
            return _scheduleRepository.Update(schedule) > 0 ? scheduledShift : null;
        }

        public IEnumerable<ScheduledShift> CreateScheduledShifts(IEnumerable<CreateScheduledShiftDTO> scheduledShiftsDto, Institution institution, int scheduleId)
        {
            var schedule = _scheduleRepository.Read(scheduleId, institution.Id);
            if (schedule == null) return null;
            var scheduledShifts = new List<ScheduledShift>();
            foreach(CreateScheduledShiftDTO scheduledShiftDto in scheduledShiftsDto)
            {
                var employees = _employeeRepository.ReadFromInstitution(institution.Id).Where(x => scheduledShiftDto.EmployeeIds.Contains(x.Id)).ToList();
                var scheduledShift = new ScheduledShift { Day = scheduledShiftDto.Day, Start = TimeSpan.Parse(scheduledShiftDto.Start), End = TimeSpan.Parse(scheduledShiftDto.End), Schedule = schedule, Employees = employees };
                scheduledShifts.Add(scheduledShift);
                schedule.Shifts.Add(scheduledShift);
            }
            return _scheduleRepository.Update(schedule) > 0 ? scheduledShifts : null;
        }

        public ScheduledShift UpdateScheduledShift(UpdateScheduledShiftDTO scheduledShiftDto, Institution institution, int scheduleId)
        {
            var dbSchedule = _scheduleRepository.Read(scheduleId, institution.Id);

            var employees = _employeeRepository.ReadFromInstitution(institution.Id).Where(x => scheduledShiftDto.EmployeeIds.Contains(x.Id)).ToList();

            dbSchedule.Shifts.Single(s => s.Id == scheduledShiftDto.Id).Employees.Clear();

            dbSchedule.Shifts.Single(s => s.Id == scheduledShiftDto.Id).Start = TimeSpan.Parse(scheduledShiftDto.Start);
            dbSchedule.Shifts.Single(s => s.Id == scheduledShiftDto.Id).End = TimeSpan.Parse(scheduledShiftDto.End);
            dbSchedule.Shifts.Single(s => s.Id == scheduledShiftDto.Id).Day = scheduledShiftDto.Day;
            dbSchedule.Shifts.Single(s => s.Id == scheduledShiftDto.Id).Employees = employees;

            _scheduleRepository.Update(dbSchedule);

            return dbSchedule.Shifts.Single(s => s.Id == scheduledShiftDto.Id);
        }

        public Schedule CreateSchedule(CreateScheduleDTO scheduleDto, Manager manager)
        {
            var schedule = new Schedule { Name = scheduleDto.Name, NumberOfWeeks = scheduleDto.NumberOfWeeks, Institution = manager.Institution, Shifts = new List<ScheduledShift>() };
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
    }
}