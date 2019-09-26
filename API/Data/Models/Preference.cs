using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class Preference
    {
        [Key]
        public int Id { get; set; }
        public int Priority { get; set; }
        [Required]
        public virtual Employee Employee { get; set; }
        [Required]
        public virtual ScheduledShift ScheduledShift { get; set; }
    }
}
