using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects.Shift
{
    public class CreateShiftDTO
    {
        /// <summary>
        /// Array of employee ids
        /// </summary>
        [Required]
        public int[] EmployeeIds { get; set; }

        /// <summary>
        /// Time the shift will start. ISO 8601 format.
        /// </summary>
        [Required]
        public string Start { get; set; }

        /// <summary>
        /// Time the shift will end. ISO 8601 format.
        /// </summary>
        [Required]
        public string End { get; set; }
    }
}
