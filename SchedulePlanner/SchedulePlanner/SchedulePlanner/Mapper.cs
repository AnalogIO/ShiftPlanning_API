using Data.DTOModels;
using Data.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchedulePlanner
{
    public class Mapper
    {
        public static IEnumerable<Shift> MapShifts(IEnumerable<ShiftDTO> shifts)
        {
            return shifts.Select((s,i) => new Shift { Id = s.Id, MaxOnShift = s.MaxOnShift, MinOnshift = s.MinOnShift, IndexId = i });
        }

        public static IEnumerable<Barista> MapPreferences(IEnumerable<PreferenceDTO> preferences)
        {
                return preferences.Select((s,i) => new Barista { Id = s.BaristaId, IndexId = i,
                    Preferences = s.Preferences.Select(p => new Preference { Priority = p.Priority, ScheduledShiftId = p.ScheduledShiftId })});
        }

        public static Tuple<int[], int[][]> MapAdditionalInfo(List<PreferenceDTO> preferences)
        {
            var friendships = new int[preferences.Count][];
            for (var i = 0; i < preferences.Count; i++)
            {
                friendships[i] = preferences.Select(p => 0).ToArray();
                foreach (var f in preferences[i].Friendships)
                {
                    var friend = preferences.SingleOrDefault(p => p.BaristaId == f);
                    if (friend == null) continue; ;
                    var index = preferences.IndexOf(friend);
                    friendships[i][index] = 5;
                }
            }

            return Tuple.Create(preferences.Select(p => p.WantShifts).ToArray(), friendships);
        }
    }
}
