using API.Logic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using API.Authorization;
using Data.Models;
using Data.Repositories;
using DataTransferObjects;
using System.Collections.Generic;

namespace API.Controllers
{
    /// <summary>
    /// Controller to manage schedule
    /// </summary>
    [RoutePrefix("api/schedules")]
    public class ScheduleController : ApiController
    {
        private readonly IScheduleRepository _scheduleRepository;
        private readonly IManagerRepository _managerRepository;

        public ScheduleController(IScheduleRepository scheduleRepository, IManagerRepository managerRepository)
        {
            _scheduleRepository = scheduleRepository;
            _managerRepository = managerRepository;
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

            var schedules = _scheduleRepository.ReadFromInstitution(manager.Institution.Id); // proper dto should be used here
            return Ok(schedules);
        }

        // GET api/schedules/{id}
        /// <summary>
        /// Gets the schedule with the given id.
        /// </summary>
        /// <param name="id">The id of the schedule.</param>
        /// <returns>
        /// Returns the schedule with the given id. 
        /// If no schedule is found with the corresponding id, the controller will return NotFound (404)
        /// </returns>
        [HttpGet, AdminFilter, Route("{id}")]
        public IHttpActionResult Get(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var manager = GetManager();
            if (manager == null) return BadRequest("Provided token is invalid!");

            var schedule = _scheduleRepository.Read(id, manager.Institution.Id);
            if (schedule != null)
            {
                return Ok(schedule);
            }
            else
            {
                return NotFound();
            }
        }

        // POST api/schedules
        /// <summary>
        /// Creates the schedule from the content in the body.
        /// </summary>
        /// <returns>
        /// Returns 'Created' (201) if the schedule gets created.
        /// </returns>
        [HttpPost, AdminFilter, Route("")]
        public IHttpActionResult Register(CreateScheduleDTO scheduleDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var manager = GetManager();
            if (manager == null) return BadRequest("Provided token is invalid!");

            var schedule = new Schedule { Name = scheduleDto.Name, NumberOfWeeks = scheduleDto.NumberOfWeeks, Institution = manager.Institution, Shifts = new List<ScheduledShift>() };
            schedule = _scheduleRepository.Create(schedule);
            if (schedule != null)
            {
                return ResponseMessage(new HttpResponseMessage(HttpStatusCode.Created));
            }
            return BadRequest("The schedule could not be created!");
        }

        private Manager GetManager()
        {
            var token = Request.Headers.Authorization.ToString();
            return _managerRepository.Read(token);
        }

    }
}