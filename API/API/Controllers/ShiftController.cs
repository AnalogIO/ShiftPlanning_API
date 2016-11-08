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

            //var shifts = _shiftRepository.GetOngoingShifts(institution.Id, DateTime.UtcNow); // to be used when shifts are implemented

            var shifts = Mapper.Map(_shiftService.GetOngoingShiftsByInstitution(institution.Id));



            /*
            var fakeOngoingShift = new ShiftDTO
            {
                Id = 1,
                Start = new DateTime(2016, 11, 4, 10, 0, 0),
                End = new DateTime(2016, 11, 4, 13, 0, 0),
                CheckedInEmployees = new List<EmployeeDTO>() { new EmployeeDTO { Id = 1, FirstName = "Frederik", LastName = "Jørgensen" } },
                Employees = new List<EmployeeDTO>(){ new EmployeeDTO { Id = 1, FirstName = "Frederik", LastName = "Jørgensen" }, new EmployeeDTO { Id = 2, FirstName = "Mark", LastName = "Rostgaard" }, new EmployeeDTO { Id = 3, FirstName = "Frederik", LastName = "Dam" } }
            };
            var fakeShifts = new List<ShiftDTO>() { fakeOngoingShift };
            */
            return Ok(new { Shifts = shifts });
        }
    }
}