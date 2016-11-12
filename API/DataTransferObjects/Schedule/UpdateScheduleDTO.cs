using Microsoft.Build.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects.Schedule
{
    public class UpdateScheduleDTO
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public int NumberOfWeeks { get; set; }
    }
}
