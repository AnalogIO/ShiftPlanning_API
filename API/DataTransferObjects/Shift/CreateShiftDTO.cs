using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects.Shift
{
    public class CreateShiftDTO
    {
        public int[] EmployeeIds { get; set; }
        public string Start { get; set; }
        public string End { get; set; }
    }
}
