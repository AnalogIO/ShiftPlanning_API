using System.Collections.Generic;
using ShiftPlanning.DTOs.Employee;

namespace ShiftPlanning.DTOs.Schedule
{
    public class ScheduledShiftDTO
    {
        public int Id { get; set; }
        public int Day { get; set; }
        public string Start { get; set; }
        public string End { get; set; }
        public int MaxOnShift { get; set; }
        public int MinOnShift { get; set; }
        public IEnumerable<EmployeeDTO> Employees { get; set; }
        public int[] LockedEmployeeIds { get; set; }
        
    }
}
