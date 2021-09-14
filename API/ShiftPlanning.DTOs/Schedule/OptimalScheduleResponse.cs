namespace ShiftPlanning.DTOs.Schedule
{
    public class OptimalScheduleResponse
    {
        public int Id { get; set; }
        public InternalShift InternalShift { get; set; }
        public Barista[] Baristas { get; set; }
    }

    public class InternalShift
    {
        public string Name { get; set; }
        public string Time { get; set; }
        public int Day { get; set; }
        public int MaxOnShift { get; set; }
        public int MultiplierNum { get; set; }
    }

    public class Barista
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

}
