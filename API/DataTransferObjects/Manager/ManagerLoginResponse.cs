using System;

namespace DataTransferObjects.Manager
{
    public class ManagerLoginResponse
    {
        public string OrganizationName { get; set; }
        public int OrganizationId { get; set; }
        public string Token { get; set; }
        public int Expires { get; set; }
    }
}