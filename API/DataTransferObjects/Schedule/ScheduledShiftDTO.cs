using DataTransferObjects.Employee;
using System;
using System.Collections.Generic;

namespace DataTransferObjects.Schedule
{
    public class ScheduledShiftDTO
    {
        public int Id { get; set; }
        public int Day { get; set; }
        public TimeSpan Start { get; set; }
        public TimeSpan End { get; set; }
        public IEnumerable<EmployeeDTO> Employees { get; set; }
    }
}
