namespace ShiftPlanning.DTOs.Employee
{
    public class EmployeeDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool Active { get; set; }
        public string EmployeeTitle { get; set; }
        public string PhotoRef { get; set; }
        public int? CheckInCount { get; set; }
        public int WantShifts { get; set; }
        public string[] Roles { get; set; }
        public int PodioId { get; set; }
    }
}
