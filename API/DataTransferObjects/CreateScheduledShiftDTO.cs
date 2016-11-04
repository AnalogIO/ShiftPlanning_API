using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects
{
    public class CreateScheduledShiftDTO
    {
        public int Day { get; set; }
        public string Start { get; set; }
        public string End { get; set; }
        public int[] EmployeeIds { get; set; }
    }
}
