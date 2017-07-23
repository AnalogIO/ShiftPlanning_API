using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DTOModels
{
    public class ScheduleDTO
    {
        public IEnumerable<ShiftDTO> Shifts { get; set; }
        public IEnumerable<PreferenceDTO> Preferences { get; set; }
    }
}
