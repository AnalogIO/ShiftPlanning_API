using Microsoft.Build.Framework;

namespace DataTransferObjects.Schedule
{
    public class CreateScheduleDTO
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public int NumberOfWeeks { get; set; }
    }
}