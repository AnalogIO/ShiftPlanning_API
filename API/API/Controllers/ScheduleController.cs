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
using Data.Models;
using Data.Services;
using DataTransferObjects.Schedule;
using DataTransferObjects.ScheduledShift;

namespace API.Controllers
{
    /// <summary>
    /// Controller to manage schedule
    /// </summary>
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
        [HttpGet, AdminFilter, Route("")]
        public IHttpActionResult Get()
        {
            var manager = _authManager.GetManagerByHeader(Request.Headers);
            if (manager == null) return BadRequest("Provided token is invalid!");

            var schedules = _scheduleService.GetSchedules(manager);
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
        [HttpGet, AdminFilter, Route("{id}")]
        public IHttpActionResult Get(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var manager = _authManager.GetManagerByHeader(Request.Headers);
            if (manager == null) return BadRequest("Provided token is invalid!");

            var schedule = _scheduleService.GetSchedule(id, manager);
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
        [HttpPost, AdminFilter, Route("")]
        public IHttpActionResult Register(CreateScheduleDTO scheduleDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var manager = _authManager.GetManagerByHeader(Request.Headers);
            if (manager == null) return BadRequest("Provided token is invalid!");

            var schedule = _scheduleService.CreateSchedule(scheduleDto, manager);
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
        [HttpDelete, AdminFilter, Route("{id}")]
        public IHttpActionResult Delete(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var manager = _authManager.GetManagerByHeader(Request.Headers);
            if (manager == null) return BadRequest("Provided token is invalid!");

            _scheduleService.DeleteSchedule(id, manager);
            return ResponseMessage(new HttpResponseMessage(HttpStatusCode.NoContent));
        }

        // PUT api/schedules/{id}
        /// <summary>
        /// Updates the schedule with the given id from the content in the body.
        /// Requires 'Authorization' header set with the token granted upon manager login.
        /// </summary>
        /// <returns>
        /// Returns 'No Content' (204) if the schedule gets updated.
        /// </returns>
        [HttpPut, AdminFilter, Route("{id}")]
        public IHttpActionResult UpdateSchedule(int id, UpdateScheduleDTO scheduleDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var manager = _authManager.GetManagerByHeader(Request.Headers);
            if (manager == null) return BadRequest("Provided token is invalid!");

            var schedule = _scheduleService.UpdateSchedule(id, scheduleDto, manager);

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
        [HttpPost, AdminFilter, Route("{id}")]
        public IHttpActionResult CreateScheduledShift(int id, CreateScheduledShiftDTO scheduledShiftDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var manager = _authManager.GetManagerByHeader(Request.Headers);
            if (manager == null) return BadRequest("Provided token is invalid!");

            var scheduledShift = _scheduleService.CreateScheduledShift(scheduledShiftDto, manager, id);

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
        [HttpPut, AdminFilter, Route("{scheduleId}/{scheduledShiftId}")]
        public IHttpActionResult UpdateScheduledShift(int scheduleId, int scheduledShiftId, UpdateScheduledShiftDTO scheduledShiftDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var manager = _authManager.GetManagerByHeader(Request.Headers);
            if (manager == null) return BadRequest("Provided token is invalid!");

            var scheduledShift = _scheduleService.UpdateScheduledShift(scheduledShiftId, scheduleId, scheduledShiftDto, manager);

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
        [HttpDelete, AdminFilter, Route("{scheduleId}/{scheduledShiftId}")]
        public IHttpActionResult DeleteScheduledShift(int scheduleId, int scheduledShiftId)
        {
            var manager = _authManager.GetManagerByHeader(Request.Headers);

            _scheduleService.DeleteScheduledShift(scheduleId, scheduledShiftId, manager);

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
        [HttpPost, AdminFilter, Route("{id}/createmultiple")]
        public IHttpActionResult CreateMultipleScheduledShift(int id, IEnumerable<CreateScheduledShiftDTO> scheduledShiftsDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var manager = _authManager.GetManagerByHeader(Request.Headers);
            if (manager == null) return BadRequest("Provided token is invalid!");

            var scheduledShifts = _scheduleService.CreateScheduledShifts(scheduledShiftsDto, manager, id);

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
        [HttpPost, AdminFilter, Route("{id}/rollout")]
        public IHttpActionResult RolloutSchedule(int id, RolloutScheduleDTO rolloutDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var manager = _authManager.GetManagerByHeader(Request.Headers);
            if (manager == null) return BadRequest("Provided token is invalid!");

            var shifts = _scheduleService.RolloutSchedule(id, rolloutDto, manager);

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
        [HttpPost, AdminFilter, Route("{id}/findoptimal")]
        public async Task<IHttpActionResult> FindOptimalSchedule(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var manager = _authManager.GetManagerByHeader(Request.Headers);
            if (manager == null) return BadRequest("Provided token is invalid!");

            var httpRequest = HttpContext.Current.Request;

            var preferenceFile = httpRequest.Files["preferences"];
            var additionalInfoFile = httpRequest.Files["additionalInfo"];
            var dislikesFile = httpRequest.Files["dislikes"];

            if (preferenceFile == null || additionalInfoFile == null || dislikesFile == null)
            {
                return BadRequest("Please provide a form with both a file input named preferences, a file input named additionalInfo and a file input named dislikes");
            }

            var schedule = _scheduleService.GetSchedule(id, manager);
            if (schedule == null) return NotFound();

            var preferenceFileContent = new StreamContent(preferenceFile.InputStream);
            var additionalInfoFileContent = new StreamContent(additionalInfoFile.InputStream);
            var dislikesFileContent = new StreamContent(dislikesFile.InputStream);
            using (var client = new HttpClient())
            using (var formData = new MultipartFormDataContent())
            {
                formData.Add(preferenceFileContent, "prefs", "prefs");
                formData.Add(additionalInfoFileContent, "ai", "ai");
                formData.Add(dislikesFileContent, "dislikes", "dislikes");
                var response = client.PostAsync($"http://80.161.174.210/scheduleplanner/api/schedule?weekCount={schedule.NumberOfWeeks}", formData).Result;
                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }


                var shifts = await response.Content.ReadAsAsync<List<OptimalScheduleResponse>>();

                var scheduledShiftList = new List<ScheduledShift>();
                var emps = _employeeService.GetEmployees(manager.Organization.Id).ToList();
                for (var i = 0; i < shifts.Count; i++)
                {
                    var scheduledShift = new ScheduledShift();
                    scheduledShift.Day = shifts[i].InternalShift.Day + (shifts[i].InternalShift.MultiplierNum-1)*7;
                    scheduledShift.Employees =
                             emps.Where(e => shifts[i].Baristas.Select(b => b.Name).Contains($"{e.FirstName} {e.LastName}"))
                                .ToList();
                    scheduledShift.Start = TimeSpan.Parse(shifts[i].InternalShift.Time.Substring(0, 5).Trim());
                    scheduledShift.End = TimeSpan.Parse(shifts[i].InternalShift.Time.Substring(8, 5).Trim());
                    if (!scheduledShift.Employees.Any()) continue;
                    scheduledShiftList.Add(scheduledShift);
                    schedule.ScheduledShifts.Add(scheduledShift);
                }

                return Ok(Mapper.Map(_scheduleService.UpdateSchedule(schedule,manager)));
                
            }
        }

    }
}