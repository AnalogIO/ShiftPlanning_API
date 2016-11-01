using System;

namespace Data.Models
{
    public class CheckIn
    {
        public int Id { get; set; }
        public DateTime Time { get; set; }
        public virtual Employee Employee { get; set; }
    }
}