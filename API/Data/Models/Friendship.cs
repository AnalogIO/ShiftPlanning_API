using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Models
{
    public class Friendship
    {
        public int Id { get; set; }

        public int Employee_Id { get; set; }
        [ForeignKey("Employee_Id")]
        public virtual Employee Employee { get; set; }
        public int Friend_Id { get; set; }

    }
}
