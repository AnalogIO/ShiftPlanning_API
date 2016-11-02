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
    [RoutePrefix("api/shifts")]
    public class ShiftController : ApiController
    {
        private readonly IShiftRepository _shiftRepository;
        private readonly int _institutionId;

        public ShiftController(IShiftRepository shiftRepository)
        {
            _shiftRepository = shiftRepository;
            _institutionId = int.Parse(ConfigurationManager.AppSettings["InstitutionId"]);
        }

        [HttpGet, Route("")]
        public IHttpActionResult Get()
        {
            return Ok(_shiftRepository.ReadFromInstitution(_institutionId));
        }

        [HttpGet, Route("ongoing"), ApiKeyFilter]
        public IHttpActionResult OnGoing()
        {
            var rand = new Random();
            return rand.NextDouble() >= 0.5 ? Ok(new { OnGoing = true }) : Ok(new { OnGoing = false });
        }
    }
}