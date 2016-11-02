using System.Configuration;
using System.Web.Http;
using Data.Repositories;
using API.Authorization;
using System;

namespace API.Controllers
{
    /// <summary>
    /// Controller to manage shifts
    /// </summary>
    [RoutePrefix("api/{institutionName}/shifts")]
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
        public IHttpActionResult Get(string institutionName)
        {
            var institution = _institutionRepository.Read(institutionName);
            if (institution == null) return BadRequest("No institution found with the given name");
            var institutionId = institution.Id;
            return Ok(_shiftRepository.ReadFromInstitution(institutionId));
        }

        [HttpGet, Route("ongoing"), ApiKeyFilter]
        public IHttpActionResult OnGoing(string institutionName)
        {
            var institution = _institutionRepository.Read(institutionName);
            if (institution == null) return BadRequest("No institution found with the given name");

            var shifts = _shiftRepository.GetOngoingShifts(institution.Id, DateTime.UtcNow);
            return Ok(shifts);
        }
    }
}