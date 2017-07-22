using System;
using DataTransferObjects.Employee;

namespace DataTransferObjects.Employee
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