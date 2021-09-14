namespace ShiftPlanning.DTOs.Employee
{
    public class EmployeeLoginResponse
    {
        public string OrganizationName { get; set; }
        public int OrganizationId { get; set; }
        public string Token { get; set; }
        public int Expires { get; set; }
        public EmployeeDTO Employee { get; set; }
    }
}