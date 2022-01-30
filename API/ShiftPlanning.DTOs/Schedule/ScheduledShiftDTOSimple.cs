namespace ShiftPlanning.DTOs.Schedule
{
    public class ScheduledShiftDTOSimple
    {
        public int Id { get; set; }
        public int Day { get; set; }
        public string Start { get; set; }
        public string End { get; set; }
    }
}