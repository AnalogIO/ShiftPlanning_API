using DataTransferObjects.Employee;
using System;

namespace DataTransferObjects.Shift
{
    public class CheckInDTO
    {
        public int Id { get; set; }
        public DateTime Time { get; set; }
        public EmployeeDTO Employee { get; set; }
    }
}
