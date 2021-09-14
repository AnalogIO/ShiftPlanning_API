using System.ComponentModel.DataAnnotations;

namespace ShiftPlanning.Model.Models
{
    public class Photo
    {
        [Key]
        public int Id { get; set; }
        public byte[] Data { get; set; }
        public string Type { get; set; }
        public virtual Organization Organization { get; set; }
    }
}
