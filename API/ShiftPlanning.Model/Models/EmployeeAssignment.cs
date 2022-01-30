using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShiftPlanning.Model.Models
{
    [Table("EmployeeAssignments")]
    public class EmployeeAssignment
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Employee_Id")]
        public virtual Employee Employee { get; set; }
        [ForeignKey("ScheduledShift_Id")]
        public virtual ScheduledShift ScheduledShift { get; set; }
        public bool IsLocked { get; set; }
    }
}
