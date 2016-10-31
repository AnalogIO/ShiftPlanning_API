using API.Data;
using API.Logic;
using API.Models;
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
        private IManagerRepository _managerRepository;

        public ScheduleController()
        {
            var context = new ShiftPlannerDataContext();
            _scheduleRepository = new ScheduleRepository(context);
            _managerRepository = new ManagerRepository(context);
        }

        // GET api/schedules
        /// <summary>
        /// Gets all the schedules.
        /// </summary>
        /// <returns>
        /// Returns an array of schedules.
        /// </returns>
        [HttpGet, AdminFilter, Route("")]
        public IHttpActionResult Get()
        {
            var manager = GetManager();
            if (manager == null) return BadRequest("Provided token is invalid!");

            var schedules = _scheduleRepository.ReadFromInstitution(manager.Institution.Id);
            return Ok(schedules);
        }

        private Manager GetManager()
        {
            var token = Request.Headers.Authorization.ToString();
            return _managerRepository.Read(token);
        }

    }
}