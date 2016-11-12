using Microsoft.Build.Framework;

namespace DataTransferObjects.Manager
{
    public class ManagerLoginDTO
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}