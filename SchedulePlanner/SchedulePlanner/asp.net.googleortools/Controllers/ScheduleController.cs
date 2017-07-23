using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using SchedulePlanner;
using Data.DTOModels;

namespace asp.net.googleortools.Controllers
{
    public class ScheduleController : ApiController
    {

        [HttpPost]
        public IHttpActionResult FindOptimalSchedule([FromBody] ScheduleDTO scheduleDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var shifts = Mapper.MapShifts(scheduleDto.Shifts);
            var prefs = Mapper.MapPreferences(scheduleDto.Preferences);
            var additionalInfo = Mapper.MapAdditionalInfo(scheduleDto.Preferences.ToList());

            var ls = new ScheduleSolver(shifts, prefs, additionalInfo, new int[prefs.ToList().Count][]);
            var schedule = ls.Solve();

            return Ok(schedule);
        }
    }
}
