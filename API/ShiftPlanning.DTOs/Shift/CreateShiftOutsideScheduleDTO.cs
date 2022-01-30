namespace ShiftPlanning.DTOs.Shift
{
    public class CreateShiftOutsideScheduleDTO
    {
        public int[] EmployeeIds { get; set; }
        public int OpenMinutes { get; set; }
    }
}
