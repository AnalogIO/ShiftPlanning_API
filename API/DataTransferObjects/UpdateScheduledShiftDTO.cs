using Microsoft.Build.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects
{
    public class UpdateScheduledShiftDTO
    {
        [Required]
        public int Id { get; set;}
        [Required]
        public int Day { get; set; }
        [Required]
        public string Start { get; set; }
        [Required]
        public string End { get; set; }
        [Required]
        public int[] EmployeeIds { get; set; }
    }
}
