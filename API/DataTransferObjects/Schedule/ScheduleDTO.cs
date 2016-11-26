using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects.Schedule
{
    public class ScheduleDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int NumberOfWeeks { get; set; }
        public IEnumerable<ScheduledShiftDTO> ScheduledShifts { get; set; }
    }
}