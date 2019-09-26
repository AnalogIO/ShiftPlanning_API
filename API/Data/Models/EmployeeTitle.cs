using System.ComponentModel.DataAnnotations;

namespace Data.Models
{
    public class EmployeeTitle
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public virtual Organization Organization { get; set; }
    }
}