using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DTOModels
{
    public class PreferenceDTO
    {
        public int BaristaId { get; set; }
        public int WantShifts { get; set; }
        public IEnumerable<Preference> Preferences { get; set; }
        public int[] Friendships { get; set; }

        public class Preference
        {
            public int ScheduledShiftId { get; set; }
            public int Priority { get; set; }
        }
    }
}
