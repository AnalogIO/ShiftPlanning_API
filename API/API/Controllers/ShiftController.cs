using System.Web.Http;
using Data.Repositories;
using API.Authorization;
using System;
using DataTransferObjects;
using System.Collections.Generic;
using Microsoft.Practices.Unity;
using API.Services;
using API.Logic;

namespace API.Controllers
{
    /// <summary>
    /// Controller to manage shifts
    /// </summary>
    [RoutePrefix("api/shifts")]
    public class ShiftController : ApiController
    {
        private readonly IShiftService _shiftService;
        private readonly IAuthManager _authManager;

        public ShiftController(IAuthManager authManager, IShiftService shiftService)
        {
            _authManager = authManager;
            _shiftService = shiftService;
        }

        [HttpGet, Route("")]
        public IHttpActionResult Get()
        {
            var institution = _authManager.GetInstitutionByHeader(Request.Headers);
            if (institution == null) return BadRequest("No institution found with the given name");
            return Ok(_shiftService.GetByInstitution(institution.Id));
        }

        [HttpGet, Route("ongoing"), ApiKeyFilter]
        public IHttpActionResult OnGoing()
        {
            var institution = _authManager.GetInstitutionByHeader(Request.Headers);
            if (institution == null) return BadRequest("No institution found with the given name");

            var shifts = Mapper.Map(_shiftService.GetOngoingShiftsByInstitution(institution.Id));

            return Ok(new { Shifts = shifts });
        }

        [HttpPost, Route("{id}/checkin"), ApiKeyFilter]
        public IHttpActionResult CheckIn(int id, int employeeId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var institution = _authManager.GetInstitutionByHeader(Request.Headers);
            if (institution == null) return BadRequest("No institution found with the given name");

            var checkIn = _shiftService.CheckInEmployee(id, employeeId, institution.Id);
            if (checkIn != null)
            {
                return Ok(new { Message = "The employee was successfully checked in!" });
            }
            return BadRequest("Could not check-in the employee - try again!");
        }
    }
}