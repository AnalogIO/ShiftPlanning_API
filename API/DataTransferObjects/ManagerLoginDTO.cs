using Microsoft.Build.Framework;

namespace DataTransferObjects
{
    public class ManagerLoginDTO
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}