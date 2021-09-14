namespace ShiftPlanning.DTOs.Shift
{
    public class PatchShiftDTO
    {
        public string Start { get; set; }
        public string End { get; set; }
        public int[] EmployeeIds { get; set; }
    }
}
