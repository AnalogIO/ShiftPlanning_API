using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects
{
    public class CheckInDTO
    {
        public int Id { get; set; }
        public DateTime Time { get; set; }
        public EmployeeDTO Employee { get; set; }
    }
}
