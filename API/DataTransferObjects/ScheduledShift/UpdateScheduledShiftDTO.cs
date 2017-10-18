using Microsoft.Build.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects.ScheduledShift
{
    public class UpdateScheduledShiftDTO
    {
        [Required]
        public int Day { get; set; }
        [Required]
        public string Start { get; set; }
        [Required]
        public string End { get; set; }
        [Required]
        public int MaxOnShift { get; set; }
        [Required]
        public int MinOnShift { get; set; }
        [Required]
        public int[] EmployeeIds { get; set; }
        [Required]
        public int[] LockedEmployeeIds { get; set; }

        
    }
}
