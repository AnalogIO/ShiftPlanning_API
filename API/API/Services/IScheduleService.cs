using System.Collections.Generic;
using Data.Models;
using DataTransferObjects.Schedule;
using DataTransferObjects.ScheduledShift;

namespace API.Logic
{
    public interface IScheduleService
    {
        Schedule CreateSchedule(CreateScheduleDTO scheduleDto, Manager manager);
        ScheduledShift CreateScheduledShift(CreateScheduledShiftDTO scheduledShiftDto, Manager manager, int scheduleId);
        IEnumerable<ScheduledShift> CreateScheduledShifts(IEnumerable<CreateScheduledShiftDTO> scheduledShiftsDto, Manager manager, int scheduleId);
        void DeleteSchedule(int scheduleId, Manager manager);
        Schedule GetSchedule(int scheduleId, Manager manager);
        IEnumerable<Schedule> GetSchedules(Manager manager);
        IEnumerable<Shift> RolloutSchedule(int scheduleId, RolloutScheduleDTO rolloutDto, Manager manager);
        Schedule UpdateSchedule(int scheduleId, UpdateScheduleDTO scheduleDto, Manager manager);
        ScheduledShift UpdateScheduledShift(int scheduledShiftId, int scheduleId, UpdateScheduledShiftDTO scheduledShiftDto, Manager manager);
    }
}