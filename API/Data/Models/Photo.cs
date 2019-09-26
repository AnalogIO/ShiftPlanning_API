using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Models
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
