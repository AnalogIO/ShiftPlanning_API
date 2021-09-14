using System.Collections.Generic;

namespace ShiftPlanning.DTOs.Schedule
{
    public class ScheduleDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int NumberOfWeeks { get; set; }
        public IEnumerable<ScheduledShiftDTO> ScheduledShifts { get; set; }
    }
}