using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects
{
    public class OngoingShiftDTO
    {
        public int Id { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public IEnumerable<EmployeeDTO> Employees { get; set; }
        public IEnumerable<EmployeeDTO> CheckedInEmployees { get; set; }
    }
}
