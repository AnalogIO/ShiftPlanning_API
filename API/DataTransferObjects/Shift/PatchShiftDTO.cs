using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects.Shift
{
    public class PatchShiftDTO
    {
        public string Start { get; set; }
        public string End { get; set; }
        public int[] EmployeeIds { get; set; }
    }
}
