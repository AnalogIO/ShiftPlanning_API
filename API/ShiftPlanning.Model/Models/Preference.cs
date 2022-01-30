using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShiftPlanning.Model.Models
{
    public class Preference
    {
        [Key]
        public int Id { get; set; }
        public int Priority { get; set; }
        [Required]
        [ForeignKey("Employee_Id")]
        public virtual Employee Employee { get; set; }
        [Required]
        [ForeignKey("ScheduledShift_Id")]
        public virtual ScheduledShift ScheduledShift { get; set; }
    }
}
