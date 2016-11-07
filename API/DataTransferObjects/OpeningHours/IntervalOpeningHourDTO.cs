using System;
using System.Collections.Generic;

namespace DataTransferObjects.OpeningHours
{
    public class IntervalOpeningHourDTO
    {
        public DateTime ShiftStart { get; set; }
        public bool Open { get; set; }
        public IEnumerable<OpeningHourEmployeeDTO> Employees { get; set; }
    }
}