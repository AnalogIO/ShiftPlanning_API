using System.ComponentModel.DataAnnotations;

namespace ShiftPlanning.DTOs.Schedule
{
    public class CreateScheduleDTO
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public int NumberOfWeeks { get; set; }
    }
}