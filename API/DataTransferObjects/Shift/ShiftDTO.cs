using DataTransferObjects.Employee;
using System;
using System.Collections.Generic;

namespace DataTransferObjects.Shift
{
    public class ShiftDTO
    {
        public int Id { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public int? ScheduleId { get; set; }
        public IEnumerable<EmployeeDTO> Employees { get; set; }
        public IEnumerable<CheckInDTO> CheckIns { get; set; }
    }
}
