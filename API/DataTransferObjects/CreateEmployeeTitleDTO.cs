using Microsoft.Build.Framework;

namespace DataTransferObjects
{
    public class CreateEmployeeTitleDTO
    {
        /// <summary>
        /// The employee title.
        /// </summary>
        [Required]
        public string Title { get; set; }
    }
}