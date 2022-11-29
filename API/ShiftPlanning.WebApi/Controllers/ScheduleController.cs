using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using ShiftPlanning.DTOs.Employee;
using ShiftPlanning.DTOs.Schedule;
using ShiftPlanning.DTOs.ScheduledShift;
using ShiftPlanning.Model.Models;
using ShiftPlanning.WebApi.Helpers.Authorization;
using ShiftPlanning.WebApi.Helpers.Mappers;
using ShiftPlanning.WebApi.Services;
using Microsoft.AspNetCore.Http;
using ShiftPlanning.DTOs.Shift;

namespace ShiftPlanning.WebApi.Controllers
{
    /// <summary>
    /// Controller to manage schedule
    /// </summary>
    [ApiController]
    [Route("api/schedules")]
    public class ScheduleController : ControllerBase
    {
        private readonly IAuthManager _authManager;
        private readonly IScheduleService _scheduleService;
        private readonly IEmployeeService _employeeService;

        /// <summary>
        /// The constructor of the schedule controller
        /// </summary>
        /// <param name="authManager"></param>
        /// <param name="scheduleService"></param>
        /// <param name="employeeService"></param>
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
        [Authorize(Roles = "Manager, Employee")]
        [HttpGet, Route("")]
        [ProducesResponseType(typeof(IEnumerable<ScheduleDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public IActionResult Get()
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
        [Authorize(Roles = "Manager, Employee")]
        [HttpGet, Route("{id}")]
        [ProducesResponseType(typeof(ScheduleDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ScheduleDTOSimple), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public IActionResult Get(int id)
        {
            var employee = _authManager.GetEmployeeByHeader(Request.Headers);
            if (employee == null) return BadRequest("Provided token is invalid!");

            var schedule = _scheduleService.GetSchedule(id, employee);
            if (schedule != null)
            {
                if (employee.Role_.Any(r => r.Name == "Manager"))
                {
                    return Ok(Mapper.Map(schedule));
                }
                else
                {
                    return Ok(Mapper.MapSimple(schedule));
                }
            }
            else
            {
                return NotFound();
            }
        }

        // GET api/schedules/{id}/preferences
        /// <summary>
        /// Gets the preferences for the schedule with the given id.
        /// Requires 'Authorization' header set with the token granted upon login.
        /// </summary>
        /// <param name="id">The id of the schedule.</param>
        /// <returns>
        /// Returns the preferences of the schedule with the given id. 
        /// If no schedule is found with the corresponding id, the controller will return NotFound (404)
        /// </returns>
        [Authorize(Roles = "Employee")]
        [HttpGet, Route("{id}/preferences")]
        [ProducesResponseType(typeof(IEnumerable<PreferenceDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public IActionResult GetPreferences(int id)
        {
            var employee = _authManager.GetEmployeeByHeader(Request.Headers);
            if (employee == null) return BadRequest("Provided token is invalid!");

            var schedule = _scheduleService.GetSchedule(id, employee);
            if (schedule != null)
            {
                return Ok(Mapper.Map(employee.Preferences.Where(p => p.ScheduledShift.Schedule?.Id == schedule.Id)));
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
        [Authorize(Roles = "Manager")]
        [HttpPost, Route("")]
        [ProducesResponseType(typeof(ScheduleDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public IActionResult Register(CreateScheduleDTO scheduleDto)
        {
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
        [Authorize(Roles = "Manager")]
        [HttpDelete, Route("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public IActionResult Delete(int id)
        {
            var employee = _authManager.GetEmployeeByHeader(Request.Headers);
            if (employee == null) return BadRequest("Provided token is invalid!");

            _scheduleService.DeleteSchedule(id, employee);
            return NoContent();
        }

        // PUT api/schedules/{id}/preferences
        /// <summary>
        /// Creates or updates the preferences from the content in the body.
        /// Requires 'Authorization' header set with the token granted upon login.
        /// </summary>
        /// <returns>
        /// Returns 'Ok' (200) if the preferences get saved.
        /// </returns>
        [Authorize(Roles = "Employee")]
        [HttpPut, Route("{id}/preferences")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public IActionResult SetPreferences(int id, IEnumerable<PreferenceDTO> preferencesDtos)
        {
            var employee = _authManager.GetEmployeeByHeader(Request.Headers);
            if (employee == null) return BadRequest("Provided token is invalid!");

            var preferences = _scheduleService.CreateOrUpdatePreferences(employee, id, preferencesDtos);
            if (preferences != null)
            {
                return NoContent();
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
        [Authorize(Roles = "Manager")]
        [HttpPut, Route("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public IActionResult UpdateSchedule(int id, UpdateScheduleDTO scheduleDto)
        {
            var employee = _authManager.GetEmployeeByHeader(Request.Headers);
            if (employee == null) return BadRequest("Provided token is invalid!");

            var schedule = _scheduleService.UpdateSchedule(id, scheduleDto, employee);

            if (schedule != null)
            {
                return NoContent();
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
        [Authorize(Roles = "Manager")]
        [HttpPost, Route("{id}")]
        [ProducesResponseType(typeof(ScheduledShiftDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public IActionResult CreateScheduledShift(int id, CreateScheduledShiftDTO scheduledShiftDto)
        {
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
        [Authorize(Roles = "Manager")]
        [HttpPut, Route("{scheduleId}/{scheduledShiftId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public IActionResult UpdateScheduledShift(int scheduleId, int scheduledShiftId, UpdateScheduledShiftDTO scheduledShiftDto)
        {
            var employee = _authManager.GetEmployeeByHeader(Request.Headers);
            if (employee == null) return BadRequest("Provided token is invalid!");

            var scheduledShift = _scheduleService.UpdateScheduledShift(scheduledShiftId, scheduleId, scheduledShiftDto, employee);

            if (scheduledShift != null)
            {
                return NoContent();
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
        [Authorize(Roles = "Manager")]
        [HttpDelete, Route("{scheduleId}/{scheduledShiftId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult DeleteScheduledShift(int scheduleId, int scheduledShiftId)
        {
            var employee = _authManager.GetEmployeeByHeader(Request.Headers);

            _scheduleService.DeleteScheduledShift(scheduleId, scheduledShiftId, employee);

            return NoContent();
        }

        // POST api/schedules/{id}/createmultiple
        /// <summary>
        /// Creates the scheduled shifts to the schedule with the given id from the content in the body.
        /// Requires 'Authorization' header set with the token granted upon manager login.
        /// </summary>
        /// <returns>
        /// Returns 'Created' (201) if the scheduled shifts gets created.
        /// </returns>
        [Authorize(Roles = "Manager")]
        [HttpPost, Route("{id}/createmultiple")]
        [ProducesResponseType(typeof(IEnumerable<ScheduledShiftDTO>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public IActionResult CreateMultipleScheduledShift(int id, IEnumerable<CreateScheduledShiftDTO> scheduledShiftsDto)
        {
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
        [Authorize(Roles = "Manager")]
        [HttpPost, Route("{id}/rollout")]
        [ProducesResponseType(typeof(IEnumerable<ShiftDTO>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public IActionResult RolloutSchedule(int id, RolloutScheduleDTO rolloutDto)
        {
            var employee = _authManager.GetEmployeeByHeader(Request.Headers);
            if (employee == null) return BadRequest("Provided token is invalid!");

            var shifts = _scheduleService.RolloutSchedule(id, rolloutDto, employee);

            if (shifts != null)
            {
                return Created($"/api/schedules/{id}", Mapper.Map(shifts));
            }

            return BadRequest("The schedule could not be created!");
        }

        //TODO Consider reimplementation, or removal
        /* 
        // POST api/schedules/{id}/findoptimal
        /// <summary>
        /// Generate the optimal schedule from the preferences set by the baristas.
        /// Requires 'Authorization' header set with the token granted upon login.
        /// </summary>
        /// <returns>
        /// Returns 'Ok' (200) if an optimal schedule can be found.
        /// </returns>
        [Authorize(Roles = "Manager")]
        [HttpPost, Route("{id}/findoptimal")]
        public async Task<IActionResult> FindOptimalSchedule(int id)
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
                var response = client.PostAsJsonAsync($"http://80.161.174.210/scheduleplanner/api/schedule/findoptimalschedule", dto).Result; //TODO FIx this shit, no hard coded IP's
                if (!response.IsSuccessStatusCode)
                {
                    return BadRequest();
                }

                //return Ok(response.RequestMessage.Content);

                var assignments = await response.Content.ReadAsAsync<List<AssignmentDTO>>();

                var emps = _employeeService.GetEmployees(employee.Organization.Id).ToList();

                foreach (var ss in schedule.ScheduledShifts)
                {
                    ss.EmployeeAssignments = ss.EmployeeAssignments.Where(ea => ea.IsLocked).ToList();
                }

                foreach(var ass in assignments)
                {
                    var ss = schedule.ScheduledShifts.FirstOrDefault(s => s.Id == ass.ShiftId);
                    ss.EmployeeAssignments.Add(new EmployeeAssignment { Employee = emps.SingleOrDefault(e => e.Id == ass.BaristaId), ScheduledShift = ss, IsLocked = false});
                }

                return Ok(Mapper.Map(_scheduleService.UpdateSchedule(schedule, employee)));
                
            }
        }*/

    }
}