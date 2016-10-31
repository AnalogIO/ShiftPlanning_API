using API.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace API.Controllers
{
    /// <summary>
    /// Controller to manage schedule
    /// </summary>
    [RoutePrefix("api/schedules")]
    public class ScheduleController : ApiController
    {
        private IScheduleRepository _scheduleRepository;

        public ScheduleController()
        {
            var context = new ShiftPlannerDataContext();
            _scheduleRepository = new ScheduleRepository(context);
        }

    }
}