using System;
using API.Logic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using API.Authorization;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Description;
using Data.Models;
using Data.Services;
using DataTransferObjects.Employee;
using DataTransferObjects.Schedule;
using DataTransferObjects.ScheduledShift;

namespace API.Controllers
{
    /// <summary>
    /// Controller to manage schedule
    /// </summary>
    [Authorize(Roles = "Manager")]
    [RoutePrefix("api/schedules")]
    public class ScheduleController : ApiController
    {
        private readonly IAuthManager _authManager;
        private readonly IScheduleService _scheduleService;
        private readonly IEmployeeService _employeeService;

        /// <summary>
        /// The constructor of the schedule controller
        /// </summary>
        /// <param name="authManager"></param>
        /// <param name="scheduleService"></param>
        public ScheduleController(IAuthManager authManager, IScheduleService scheduleService, IEmployeeService employeeService)
        {
            _authManager = authManager;
            _scheduleService = scheduleService;
            _employeeService = employeeService;
        }

        // GET api/schedules
        /// <summary>
        /// Gets all the schedules.
        /// Requires 'Authorization' header set with the token granted upon manager login.
        /// </summary>
        /// <returns>
        /// Returns an array of schedules.
        /// </returns>
        [HttpGet, Route("")]
        public IHttpActionResult Get()
        {
            var employee = _authManager.GetEmployeeByHeader(Request.Headers);
            if (employee == null) return BadRequest("Provided token is invalid!");

            var schedules = _scheduleService.GetSchedules(employee);
            return Ok(Mapper.Map(schedules));
        }

        // GET api/schedules/{id}
        /// <summary>
        /// Gets the schedule with the given id.
        /// Requires 'Authorization' header set with the token granted upon manager login.
        /// </summary>
        /// <param name="id">The id of the schedule.</param>
        /// <returns>
        /// Returns the schedule with the given id. 
        /// If no schedule is found with the corresponding id, the controller will return NotFound (404)
        /// </returns>
        [HttpGet, Route("{id}")]
        public IHttpActionResult Get(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var employee = _authManager.GetEmployeeByHeader(Request.Headers);
            if (employee == null) return BadRequest("Provided token is invalid!");

            var schedule = _scheduleService.GetSchedule(id, employee);
            if (schedule != null)
            {
                return Ok(Mapper.Map(schedule));
            }
            else
            {
                return NotFound();
            }
        }

        // POST api/schedules
        /// <summary>
        /// Creates the schedule from the content in the body.
        /// Requires 'Authorization' header set with the token granted upon manager login.
        /// </summary>
        /// <returns>
        /// Returns 'Created' (201) if the schedule gets created.
        /// </returns>
        [HttpPost, Route("")]
        public IHttpActionResult Register(CreateScheduleDTO scheduleDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var employee = _authManager.GetEmployeeByHeader(Request.Headers);
            if (employee == null) return BadRequest("Provided token is invalid!");

            var schedule = _scheduleService.CreateSchedule(scheduleDto, employee);
            if (schedule != null)
            {
                return Created($"/api/schedules/{schedule.Id}", Mapper.Map(schedule));
            }
            return BadRequest("The schedule could not be created!");
        }

        // DELETE /api/schedules/{id}
        /// <summary>
        /// Deletes the schedule with the specified id.
        /// Requires 'Authorization' header set with the token granted upon manager login.
        /// </summary>
        /// <param name="id">The id of the schedule.</param>
        /// <returns>Returns 'No Content' (204) if the schedule gets deleted.</returns>
        [HttpDelete, Route("{id}")]
        public IHttpActionResult Delete(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var employee = _authManager.GetEmployeeByHeader(Request.Headers);
            if (employee == null) return BadRequest("Provided token is invalid!");

            _scheduleService.DeleteSchedule(id, employee);
            return ResponseMessage(new HttpResponseMessage(HttpStatusCode.NoContent));
        }

        // POST api/schedules/{id}/preferences
        /// <summary>
        /// Creates or updates the preferences from the content in the body.
        /// Requires 'Authorization' header set with the token granted upon login.
        /// </summary>
        /// <returns>
        /// Returns 'Ok' (200) if the preferences get saved.
        /// </returns>
        [Authorize(Roles = "Employee")]
        [HttpPost, Route("{id}/setpreferences")]
        [ResponseType(typeof(IEnumerable<PreferenceDTO>))]
        public IHttpActionResult SetPreferences(int id, IEnumerable<PreferenceDTO> preferencesDtos)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var employee = _authManager.GetEmployeeByHeader(Request.Headers);
            if (employee == null) return BadRequest("Provided token is invalid!");

            var preferences = _scheduleService.CreateOrUpdatePreferences(employee, id, preferencesDtos);
            if (preferences != null)
            {
                return Ok(Mapper.Map(preferences));
            }
            return BadRequest("The preferences could not be created or updated!");
        }

        // PUT api/schedules/{id}
        /// <summary>
        /// Updates the schedule with the given id from the content in the body.
        /// Requires 'Authorization' header set with the token granted upon manager login.
        /// </summary>
        /// <returns>
        /// Returns 'No Content' (204) if the schedule gets updated.
        /// </returns>
        [HttpPut, Route("{id}")]
        public IHttpActionResult UpdateSchedule(int id, UpdateScheduleDTO scheduleDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var employee = _authManager.GetEmployeeByHeader(Request.Headers);
            if (employee == null) return BadRequest("Provided token is invalid!");

            var schedule = _scheduleService.UpdateSchedule(id, scheduleDto, employee);

            if (schedule != null)
            {
                return ResponseMessage(new HttpResponseMessage(HttpStatusCode.NoContent));
            }

            return BadRequest("The schedule could not be updated!");
        }

        // POST api/schedules/{id}
        /// <summary>
        /// Creates the scheduled shift to the schedule with the given id from the content in the body.
        /// Requires 'Authorization' header set with the token granted upon manager login.
        /// </summary>
        /// <returns>
        /// Returns 'Created' (201) if the scheduled shift gets created.
        /// </returns>
        [HttpPost, Route("{id}")]
        public IHttpActionResult CreateScheduledShift(int id, CreateScheduledShiftDTO scheduledShiftDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var employee = _authManager.GetEmployeeByHeader(Request.Headers);
            if (employee == null) return BadRequest("Provided token is invalid!");

            var scheduledShift = _scheduleService.CreateScheduledShift(scheduledShiftDto, employee, id);

            if (scheduledShift != null) {
                return Created($"/api/schedules/{id}", Mapper.Map(scheduledShift));
            }

            return BadRequest("The schedule could not be created!");
        }

        // PUT api/schedules/{scheduleId}/{scheduledShiftId}
        /// <summary>
        /// Updates the scheduled shift with the given id from the content in the body.
        /// Requires 'Authorization' header set with the token granted upon manager login.
        /// </summary>
        /// <returns>
        /// Returns 'No Content' (204) if the scheduled shift gets updated.
        /// </returns>
        [HttpPut, Route("{scheduleId}/{scheduledShiftId}")]
        public IHttpActionResult UpdateScheduledShift(int scheduleId, int scheduledShiftId, UpdateScheduledShiftDTO scheduledShiftDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var employee = _authManager.GetEmployeeByHeader(Request.Headers);
            if (employee == null) return BadRequest("Provided token is invalid!");

            var scheduledShift = _scheduleService.UpdateScheduledShift(scheduledShiftId, scheduleId, scheduledShiftDto, employee);

            if (scheduledShift != null)
            {
                return ResponseMessage(new HttpResponseMessage(HttpStatusCode.NoContent));
            }

            return BadRequest("The scheduled shift could not be updated!");
        }

        // PUT api/schedules/{scheduleId}/{scheduledShiftId}
        /// <summary>
        /// Deletes the scheduled shift with the given id.
        /// Requires 'Authorization' header set with the token granted upon manager login.
        /// </summary>
        /// <returns>
        /// Returns 'No Content' (204) if the scheduled shift gets deleted.
        /// </returns>
        [HttpDelete, Route("{scheduleId}/{scheduledShiftId}")]
        public IHttpActionResult DeleteScheduledShift(int scheduleId, int scheduledShiftId)
        {
            var employee = _authManager.GetEmployeeByHeader(Request.Headers);

            _scheduleService.DeleteScheduledShift(scheduleId, scheduledShiftId, employee);

            return ResponseMessage(new HttpResponseMessage(HttpStatusCode.NoContent));
        }

        // POST api/schedules/{id}/createmultiple
        /// <summary>
        /// Creates the scheduled shifts to the schedule with the given id from the content in the body.
        /// Requires 'Authorization' header set with the token granted upon manager login.
        /// </summary>
        /// <returns>
        /// Returns 'Created' (201) if the scheduled shifts gets created.
        /// </returns>
        [HttpPost, Route("{id}/createmultiple")]
        public IHttpActionResult CreateMultipleScheduledShift(int id, IEnumerable<CreateScheduledShiftDTO> scheduledShiftsDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var employee = _authManager.GetEmployeeByHeader(Request.Headers);
            if (employee == null) return BadRequest("Provided token is invalid!");

            var scheduledShifts = _scheduleService.CreateScheduledShifts(scheduledShiftsDto, employee, id);

            if (scheduledShifts != null)
            {
                return Created($"/api/schedules/{id}", Mapper.Map(scheduledShifts));
            }

            return BadRequest("The schedule could not be created!");
        }

        // POST api/schedules/{id}/rollout
        /// <summary>
        /// Creates shifts from the set schedule id in the interval set in the body with from and to datetimes.
        /// Requires 'Authorization' header set with the token granted upon manager login.
        /// </summary>
        /// <returns>
        /// Returns 'Created' (201) if the scheduled gets rolled out.
        /// </returns>
        [HttpPost, Route("{id}/rollout")]
        public IHttpActionResult RolloutSchedule(int id, RolloutScheduleDTO rolloutDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var employee = _authManager.GetEmployeeByHeader(Request.Headers);
            if (employee == null) return BadRequest("Provided token is invalid!");

            var shifts = _scheduleService.RolloutSchedule(id, rolloutDto, employee);

            if (shifts != null)
            {
                return Created($"/api/schedules/{id}", Mapper.Map(shifts));
            }

            return BadRequest("The schedule could not be created!");
        }


        // POST api/schedules/{id}/findoptimal
        /// <summary>
        /// Find the optimal schedule from the given input CSV file.
        /// Requires 'Authorization' header set with the token granted upon manager login.
        /// </summary>
        /// <returns>
        /// Returns 'Ok' (200) if an optimal schedule can be found.
        /// </returns>
        [HttpPost, Route("{id}/findoptimal")]
        public async Task<IHttpActionResult> FindOptimalSchedule(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var employee = _authManager.GetEmployeeByHeader(Request.Headers);
            if (employee == null) return BadRequest("Provided token is invalid!");

            var schedule = _scheduleService.GetSchedule(id, employee);
            if (schedule == null) return NotFound();

            using (var client = new HttpClient()) {
                var dto = Mapper.MapToFindOptimalScheduleDto(schedule);
                var response = client.PostAsJsonAsync($"http://80.161.174.210/scheduleplanner/api/schedule/findoptimalschedule", dto).Result;
                if (!response.IsSuccessStatusCode)
                {
                    return BadRequest();
                }

                var assignments = await response.Content.ReadAsAsync<List<AssignmentDTO>>();

                var emps = _employeeService.GetEmployees(employee.Organization.Id).ToList();

                foreach(var ss in schedule.ScheduledShifts) ss.Employees.Clear();

                foreach(var ass in assignments)
                {
                    var ss = schedule.ScheduledShifts.FirstOrDefault(s => s.Id == ass.ShiftId);
                    ss.Employees.Add(emps.SingleOrDefault(e => e.Id == ass.BaristaId));
                }

                return Ok(Mapper.Map(_scheduleService.UpdateSchedule(schedule, employee)));
                
            }
        }

    }
}