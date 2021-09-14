using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShiftPlanning.DTOs.Public.OpeningHours;
using ShiftPlanning.DTOs.Shift;
using ShiftPlanning.WebApi.Helpers.Authorization;
using ShiftPlanning.WebApi.Helpers.Mappers;
using ShiftPlanning.WebApi.Services;

namespace ShiftPlanning.WebApi.Controllers
{
    /// <summary>
    /// The ShiftsController is the public entrance to get opening hour and shift information.
    /// </summary>
    [ApiController]
    [Route("api/shifts")]
    public class ShiftsController : ControllerBase
    {
        private readonly IShiftService _shiftService;
        private readonly IOpeningHoursMapper _mapper;
        private readonly IAuthManager _authManager;

        /// <summary>
        /// Dependency injection constructor.
        /// </summary>
        /// <param name="authManager"></param>
        /// <param name="shiftService"></param>
        /// <param name="mapper"></param>
        public ShiftsController(IAuthManager authManager, IShiftService shiftService, IOpeningHoursMapper mapper)
        {
            _shiftService = shiftService;
            _mapper = mapper;
            _authManager = authManager;
        }

        /// <summary>
        /// Fetch shifts for the next week for a given organization.
        /// </summary>
        /// <param name="shortKey">ShortKey of the institution to fetch opening hours for.</param>
        /// <returns>A collection of OpeningHoursDTO. NotFound if organizationRepository was not found.</returns>
        [HttpGet, Route("{shortKey}")]
        [ProducesResponseType(typeof(IEnumerable<OpeningHoursDTO>), 200)]
        public IActionResult Get(string shortKey)
        {
            //var monday = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Monday);
            var sunday = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek);
            var shifts = _shiftService.GetByOrganization(shortKey, sunday, sunday.AddDays(7)).OrderBy(s => s.Start).ToList();

            if (shifts.Count == 0)
            {
                return NotFound();
            }

            return Ok(_mapper.MapToDto(shifts));
        }

        /// <summary>
        /// Fetch shifts for today.
        /// </summary>
        /// <param name="shortKey">ShortKey of the organization to fetch today's shifts for.</param>
        /// <returns>A collection of OpeningHoursDTO. NotFound if organizationRepository was not found.</returns>
        [HttpGet, Route("today/{shortKey}")]
        [ProducesResponseType(typeof(IEnumerable<OpeningHoursDTO>), 200)]
        public IActionResult GetToday(string shortKey)
        {
            //var monday = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Monday);
            var today = DateTime.Now;

            var shifts = _shiftService.GetByOrganization(shortKey, today);

            if (shifts == null) return NotFound();

            return Ok(_mapper.MapToDto(shifts));
        }

        /// <summary>
        /// Fetch shifts for the next week for a given organization.
        /// </summary>
        /// <param name="shortKey">ShortKey of the organization to fetch opening hours for.</param>
        /// <param name="interval">The number of minutes an interval should span.</param>
        /// <returns>A collection of IntervalOpeningHoursDTO. NotFound if organizationRepository was not found.</returns>
        [HttpGet, Route("~/api/openinghours/{shortKey}")]
        [ProducesResponseType(typeof(IntervalOpeningHoursDTO), 200)]
        public IActionResult GetIntervals(string shortKey, int interval = 30)
        {
            //var monday = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Monday);
            var sunday = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek);
            var shifts = _shiftService.GetByOrganization(shortKey, sunday, sunday.AddDays(7)).OrderBy(s => s.Start)?.ToList();

            if (shifts.Count == 0) return NotFound();

            return Ok(_mapper.MapToIntervalDto(shifts, interval));
        }

        /// <summary>
        /// Returns all shifts of the specified organization in the 'Authorization' header
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Manager")]
        [HttpGet, Route("")]
        public IActionResult Get(string from, string to)
        {
            var employee = _authManager.GetEmployeeByHeader(Request.Headers);
            if (employee == null) return BadRequest("No manager found with the given name");

            var dtFrom = Convert.ToDateTime(from);
            var dtTo = Convert.ToDateTime(to);

            return Ok(Mapper.Map(_shiftService.GetByOrganization(employee.Organization.Id, dtFrom, dtTo)));
        }

        /// <summary>
        /// Returns the shift for the given id in the parameter
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Manager, Application")]
        [HttpGet, Route("{id:int}")]
        public IActionResult Get(int id)
        {
            var organization = _authManager.GetOrganizationByHeader(Request.Headers);
            if (organization == null) return BadRequest("No institution found with the given name");

            var shift = _shiftService.GetShift(id, organization.Id);
            if(shift != null)
            {
                return Ok(Mapper.Map(shift));
            }
            return NotFound();
        }

        /// <summary>
        /// Deletes the shift with the given id in the parameter
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Manager, Application")]
        [HttpDelete, Route("{id:int}")]
        public IActionResult Delete(int id)
        {
            var organization = _authManager.GetOrganizationByHeader(Request.Headers);
            if (organization == null) return BadRequest("No manager found with the given token");

            _shiftService.DeleteShift(id, organization.Id);
            
            return NoContent();
        }

        /// <summary>
        /// Updates the shift with the id in the parameter with the content in the body
        /// </summary>
        /// <param name="id"></param>
        /// <param name="shiftDto"></param>
        /// <returns></returns>
        [Authorize(Roles = "Manager, Application")]
        [HttpPut, Route("{id:int}")]
        public IActionResult Update(int id, UpdateShiftDTO shiftDto)
        {
            var organization = _authManager.GetOrganizationByHeader(Request.Headers);
            if (organization == null) return BadRequest("No manager found with the given token");

            var shift = _shiftService.UpdateShift(id, organization.Id, shiftDto);

            if(shift != null)
            {
                return NoContent();
            }

            return BadRequest("The shift could not be updated!");
        }

        /// <summary>
        /// Patches the shift with the id in the parameter with the content in the body
        /// </summary>
        /// <param name="id"></param>
        /// <param name="shiftDto"></param>
        /// <returns></returns>
        [Authorize(Roles = "Manager, Application")]
        [HttpPatch, Route("{id:int}")]
        public IActionResult Patch(int id, PatchShiftDTO shiftDto)
        {
            var organization = _authManager.GetOrganizationByHeader(Request.Headers);
            if (organization == null) return BadRequest("No manager found with the given token");

            var shift = _shiftService.PatchShift(id, organization.Id, shiftDto);

            if (shift != null)
            {
                return NoContent();
            }

            return BadRequest("The shift could not be updated!");
        }

        /// <summary>
        /// Creates a shift with the given employees for the given time defined in the body
        /// </summary>
        /// <param name="shiftDto"></param>
        /// <returns></returns>
        [Authorize(Roles = "Manager, Application")]
        [HttpPost, Route("")]
        public IActionResult Create(CreateShiftDTO shiftDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var organization = _authManager.GetOrganizationByHeader(Request.Headers);
            if (organization == null) return BadRequest("Token/apikey is not assigned any organization");

            var shift = _shiftService.CreateShift(organization, shiftDto);
            if (shift != null)
            {
                return Ok(Mapper.Map(shift));
            }
            return BadRequest("Could not create shift!");
        }

        /// <summary>
        /// Gets the shifts for today
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Application")]
        [HttpGet, Route("today")]
        public IActionResult Today()
        {
            var organization = _authManager.GetOrganizationByHeader(Request.Headers);
            if (organization == null) return BadRequest("No institution found with the given name");

            var now = DateTime.Now.Date;
            var end = now.AddDays(1).AddTicks(-1);

            var shifts = Mapper.Map(_shiftService.GetByOrganization(organization.Id,now,end));

            return Ok(shifts);
        }


        /// <summary>
        /// Gets the shifts currently ongoing with the corresponding employees planned to be on the shift and the employees checked-in on that shift
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Application")]
        [HttpGet, Route("ongoing")]
        public IActionResult OnGoing()
        {
            var organization = _authManager.GetOrganizationByHeader(Request.Headers);
            if (organization == null) return BadRequest("No institution found with the given name");

            var shifts = Mapper.Map(_shiftService.GetOngoingShiftsByOrganization(organization.Id));

            return Ok(new { Shifts = shifts });
        }

        /// <summary>
        /// Checks in the employee with the given employee id in the parameters for the given shift id in the parameters
        /// </summary>
        /// <param name="id"></param>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        [Authorize(Roles = "Application")]
        [HttpPost, Route("{id}/checkin")]
        public IActionResult CheckIn(int id, int employeeId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var organization = _authManager.GetOrganizationByHeader(Request.Headers);
            if (organization == null) return BadRequest("No institution found with the given name");

            var checkIn = _shiftService.CheckInEmployee(id, employeeId, organization.Id);
            if (checkIn != null)
            {
                return Ok(Mapper.Map(checkIn));
            }
            return BadRequest("Could not check-in the employee - try again!");
        }

        /// <summary>
        /// Checks out the employee with the given employee id in the parameters for the given shift id in the parameters
        /// </summary>
        /// <param name="id"></param>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        [Authorize(Roles = "Application")]
        [HttpPost, Route("{id}/checkout")]
        public IActionResult CheckOut(int id, int employeeId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var organization = _authManager.GetOrganizationByHeader(Request.Headers);
            if (organization == null) return BadRequest("No institution found with the given name");

            _shiftService.CheckOutEmployee(id, employeeId, organization.Id);

            return Ok(Mapper.Map("Checked out with success!"));
        }
    }
}