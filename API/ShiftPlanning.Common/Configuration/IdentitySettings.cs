using System.ComponentModel.DataAnnotations;
using NetEscapades.Configuration.Validation;

namespace ShiftPlanning.Common.Configuration
{
    public class IdentitySettings : IValidatable
    {
        [Required]
        public string TokenKey { get; set; }
        
        [Required]
        public int TokenAgeHour { get; set; }
        
        public void Validate()
        {
            Validator.ValidateObject(this, new ValidationContext(this), validateAllProperties: true);
        }
    }
}