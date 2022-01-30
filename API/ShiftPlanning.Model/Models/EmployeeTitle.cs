using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShiftPlanning.Model.Models
{
    public class EmployeeTitle
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        [ForeignKey("Organization_Id")]
        public virtual Organization Organization { get; set; }
    }
}