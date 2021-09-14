using System;
using ShiftPlanning.DTOs.Employee;

namespace ShiftPlanning.DTOs.Shift
{
    public class CheckInDTO
    {
        public int Id { get; set; }
        public DateTime Time { get; set; }
        public EmployeeDTO Employee { get; set; }
    }
}
