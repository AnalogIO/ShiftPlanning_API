using System;
using System.Collections.Generic;

namespace DataTransferObjects.Public.OpeningHours
{
    public class IntervalOpeningHourDTO
    {
        public DateTime ShiftStart { get; set; }
        public bool Open { get; set; }
        public IEnumerable<string> Employees { get; set; }
    }
}