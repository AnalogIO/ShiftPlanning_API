using Data.Models;
using Data.Repositories;
using DataTransferObjects;
using Microsoft.Practices.Unity;
using System;
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
    }
}