using DataTransferObjects.Employee;
using System;
using System.Collections.Generic;

namespace DataTransferObjects.Schedule
{
    public class ScheduledShiftDTO
    {
        public int Id { get; set; }
        public int Day { get; set; }
        public string Start { get; set; }
        public string End { get; set; }
        public int MaxOnShift { get; set; }
        public IEnumerable<EmployeeDTO> Employees { get; set; }
    }
}
