using System.Collections.Generic;
using Data.Models;
using DataTransferObjects.Schedule;
using DataTransferObjects.ScheduledShift;

namespace Data.Services
{
    public interface IScheduleService
    {
        Schedule CreateSchedule(CreateScheduleDTO scheduleDto, Employee employee);
        ScheduledShift CreateScheduledShift(CreateScheduledShiftDTO scheduledShiftDto, Employee employee, int scheduleId);
        IEnumerable<ScheduledShift> CreateScheduledShifts(IEnumerable<CreateScheduledShiftDTO> scheduledShiftsDto, Employee employee, int scheduleId);
        void DeleteSchedule(int scheduleId, Employee employee);
        Schedule GetSchedule(int scheduleId, Employee employee);
        IEnumerable<Schedule> GetSchedules(Employee employee);
        IEnumerable<Shift> RolloutSchedule(int scheduleId, RolloutScheduleDTO rolloutDto, Employee employee);
        Schedule UpdateSchedule(int scheduleId, UpdateScheduleDTO scheduleDto, Employee employee);
        Schedule UpdateSchedule(Schedule schedule, Employee employee);
        ScheduledShift UpdateScheduledShift(int scheduledShiftId, int scheduleId, UpdateScheduledShiftDTO scheduledShiftDto, Employee employee);
        void DeleteScheduledShift(int scheduleId, int scheduledShiftId, Employee employee);
    }
}