using System.Configuration;
using System.Web.Http;
using Data.Repositories;
using API.Authorization;
using System;
using DataTransferObjects;
using System.Collections.Generic;
using Data.Models;

namespace API.Controllers
{
    /// <summary>
    /// Controller to manage shifts
    /// </summary>
    [RoutePrefix("api/shifts")]
    public class ShiftController : ApiController
    {
        private readonly IShiftRepository _shiftRepository;
        private readonly IInstitutionRepository _institutionRepository;

        public ShiftController(IShiftRepository shiftRepository, IInstitutionRepository institutionRepository)
        {
            _shiftRepository = shiftRepository;
            _institutionRepository = institutionRepository;
        }

        [HttpGet, Route("")]
        public IHttpActionResult Get()
        {
            var institution = GetInstitution();
            if (institution == null) return BadRequest("No institution found with the given name");
            var institutionId = institution.Id;
            return Ok(_shiftRepository.ReadFromInstitution(institutionId));
        }

        [HttpGet, Route("ongoing"), ApiKeyFilter]
        public IHttpActionResult OnGoing()
        {
            var institution = GetInstitution();
            if (institution == null) return BadRequest("No institution found with the given name");

            //var shifts = _shiftRepository.GetOngoingShifts(institution.Id, DateTime.UtcNow); // to be used when shifts are implemented

            var fakeOngoingShift = new OngoingShiftDTO
            {
                Id = 1,
                Start = new DateTime(2016, 11, 4, 10, 0, 0),
                End = new DateTime(2016, 11, 4, 13, 0, 0),
                CheckedInEmployees = new List<EmployeeDTO>() { new EmployeeDTO { Id = 1, FirstName = "Frederik", LastName = "Jørgensen" } },
                Employees = new List<EmployeeDTO>(){ new EmployeeDTO { Id = 1, FirstName = "Frederik", LastName = "Jørgensen" }, new EmployeeDTO { Id = 2, FirstName = "Mark", LastName = "Rostgaard" }, new EmployeeDTO { Id = 3, FirstName = "Frederik", LastName = "Dam" } }
            };
            var fakeShifts = new List<OngoingShiftDTO>() { fakeOngoingShift };
            return Ok(new { Shifts = fakeShifts });
        }

        private Institution GetInstitution()
        {
            var token = Request.Headers.Authorization.ToString();
            return _institutionRepository.Read(token);
        }
    }
}