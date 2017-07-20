using System;
using System.Collections.Generic;
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
        public int[] EmployeeIds { get; set; }
        
        /// <summary>
        /// Time the shift will start. ISO 8601 format.
        /// </summary>
        public string Start { get; set; }
        
        /// <summary>
        /// Time the shift will end. ISO 8601 format.
        /// </summary>
        public string End { get; set; }
    }
}
