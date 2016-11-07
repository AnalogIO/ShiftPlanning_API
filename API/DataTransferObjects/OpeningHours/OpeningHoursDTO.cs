using System;
using System.Collections.Generic;

namespace DataTransferObjects.OpeningHours
{
    public class OpeningHoursDTO
    {
        public int Id { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public IEnumerable<OpeningHourEmployeeDTO> Employees { get; set; }
        public IEnumerable<OpeningHourEmployeeDTO> CheckedInEmployees { get; set; }
    }
}
