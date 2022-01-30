using System.ComponentModel.DataAnnotations;
using NetEscapades.Configuration.Validation;

namespace ShiftPlanning.Common.Configuration
{
    public class PodioSettings : IValidatable
    {
        [Required] public string PodioClientId { get; set; }
        [Required] public string PodioClientSecret { get; set; }
        [Required] public int PodioAppId { get; set; }
        [Required] public string PodioAppToken { get; set; }

        public void Validate()
        {
            Validator.ValidateObject(this, new ValidationContext(this), true);
        }
    }
}