using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects.Shift
{
    public class UpdateShiftDTO
    {
        [Required]
        public string Start { get; set; }
        [Required]
        public string End { get; set; }
        [Required]
        public int[] EmployeeIds { get; set; }
    }
}
