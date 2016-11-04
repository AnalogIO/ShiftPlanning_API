using API.Logic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using API.Authorization;
using Data.Models;
using Data.Repositories;
using DataTransferObjects;
using System.Collections.Generic;
using Microsoft.Practices.Unity;

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
        private readonly AuthManager _authManager;
        private readonly ScheduleManager _scheduleManager;

        public ScheduleController(IScheduleRepository scheduleRepository, IManagerRepository managerRepository, IInstitutionRepository institutionRepository, IEmployeeRepository employeeRepository)
        {
            _scheduleRepository = scheduleRepository;
            _managerRepository = managerRepository;
            _authManager = new AuthManager(institutionRepository, managerRepository);
            _scheduleManager = new ScheduleManager(scheduleRepository, institutionRepository, employeeRepository);
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
            var manager = _authManager.GetManagerByHeader(Request.Headers);
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

            var manager = _authManager.GetManagerByHeader(Request.Headers);
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

            var manager = _authManager.GetManagerByHeader(Request.Headers);
            if (manager == null) return BadRequest("Provided token is invalid!");

            var schedule = new Schedule { Name = scheduleDto.Name, NumberOfWeeks = scheduleDto.NumberOfWeeks, Institution = manager.Institution, Shifts = new List<ScheduledShift>() };
            schedule = _scheduleRepository.Create(schedule);
            if (schedule != null)
            {
                return ResponseMessage(new HttpResponseMessage(HttpStatusCode.Created));
            }
            return BadRequest("The schedule could not be created!");
        }

        // POST api/schedules/{id}
        /// <summary>
        /// Creates the scheduled shift to the schedule with the given id from the content in the body.
        /// </summary>
        /// <returns>
        /// Returns 'Created' (201) if the scheduled shift gets created.
        /// </returns>
        [HttpPost, AdminFilter, Route("{id}")]
        public IHttpActionResult CreateScheduledShift(int id, CreateScheduledShiftDTO scheduledShiftDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var manager = _authManager.GetManagerByHeader(Request.Headers);
            if (manager == null) return BadRequest("Provided token is invalid!");

            var scheduledShift = _scheduleManager.CreateScheduledShift(scheduledShiftDto, manager.Institution, id);

            if (scheduledShift != null) {
                return ResponseMessage(new HttpResponseMessage(HttpStatusCode.Created));
            }

            return BadRequest("The schedule could not be created!");
        }

    }
}