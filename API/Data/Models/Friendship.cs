using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class Friendship
    {
        public int Id { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual Employee Friend { get; set; }
    }
}
