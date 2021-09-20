﻿using System.ComponentModel.DataAnnotations;

namespace ShiftPlanning.DTOs.ScheduledShift
{
    public class UpdateScheduledShiftDTO
    {
        [Required]
        public int Day { get; set; }
        [Required]
        public string Start { get; set; }
        [Required]
        public string End { get; set; }
        [Required]
        public int MaxOnShift { get; set; }
        [Required]
        public int MinOnShift { get; set; }
        [Required]
        public int[] EmployeeIds { get; set; }
        [Required]
        public int[] LockedEmployeeIds { get; set; }

        
    }
}