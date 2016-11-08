using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects.Shift
{
    public class CreateShiftOutsideScheduleDTO
    {
        public int[] EmployeeIds { get; set; }
        public int OpenMinutes { get; set; }
    }
}
