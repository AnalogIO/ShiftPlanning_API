using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects.Schedule
{
    public class FindOptimalScheduleDTO
    {
        public IEnumerable<FindOptimalScheduleShiftDTO> Shifts { get; set; }
        public IEnumerable<FindOptimalSchedulePreferencesDTO> Preferences { get; set; }
    }

    public class FindOptimalScheduleShiftDTO
    {
        public int Id { get; set; }
        public int MaxOnShift { get; set; }
        public int MinOnShift { get; set; }
    }

    public class FindOptimalSchedulePreferencesDTO
    {
        public int BaristaId { get; set; }
        public int WantShifts { get; set; }
        public IEnumerable<FindOptimalSchedulePreference> Preferences { get; set; }
        public int[] Friendships { get; set; }

        public class FindOptimalSchedulePreference
        {
            public int ScheduledShiftId { get; set; }
            public int Priority { get; set; }
        }

    }

}
