using System.ComponentModel.DataAnnotations;

namespace ShiftPlanning.DTOs.Schedule
{
    public class UpdateScheduleDTO
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public int NumberOfWeeks { get; set; }
    }
}
