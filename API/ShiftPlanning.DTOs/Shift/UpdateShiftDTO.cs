using System.ComponentModel.DataAnnotations;

namespace ShiftPlanning.DTOs.Shift
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
