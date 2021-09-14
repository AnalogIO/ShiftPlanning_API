using System.ComponentModel.DataAnnotations;
using NetEscapades.Configuration.Validation;

namespace ShiftPlanning.Common.Configuration
{
    public class FtpSettings : IValidatable
    {
        [Required] public string FtpHost { get; set; }
        [Required] public string FtpUsername { get; set; }
        [Required] public string FtpPassword { get; set; }
        
        public void Validate()
        {
            Validator.ValidateObject(this, new ValidationContext(this), true);
        }
    }
}