using System.ComponentModel.DataAnnotations;

namespace ShiftPlanning.DTOs.Shift
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
