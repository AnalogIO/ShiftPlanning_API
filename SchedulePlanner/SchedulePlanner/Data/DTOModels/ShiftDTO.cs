using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DTOModels
{
    public class ShiftDTO
    {
        public int Id { get; set; }
        public int MaxOnShift { get; set; }
        public int MinOnShift { get; set; }
    }
}
