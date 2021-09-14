using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShiftPlanning.Model.Models
{
    public class CheckIn
    {
        [Key]
        public int Id { get; set; }
        public DateTime Time { get; set; }
        [ForeignKey("Employee_Id")]
        public virtual Employee Employee { get; set; }
        [ForeignKey("Shift_Id")]
        public virtual Shift Shift { get; set; }
    }
}