using System.Web.Http;
using API.Authorization;
using API.Logic;
using DataTransferObjects.Shift;
using System.Net.Http;
using System.Net;
using Data.Services;

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

        [HttpGet, Route("{id}")]
        public IHttpActionResult Get(int id)
        {
            var institution = _authManager.GetInstitutionByHeader(Request.Headers);
            if (institution == null) return BadRequest("No institution found with the given name");

            var shift = _shiftService.GetShift(id, institution.Id);
            if(shift != null)
            {
                return Ok(Mapper.Map(shift));
            }
            return NotFound();
        }

        [HttpDelete, Route("{id}")]
        public IHttpActionResult Delete(int id)
        {
            var institution = _authManager.GetInstitutionByHeader(Request.Headers);
            if (institution == null) return BadRequest("No institution found with the given name");

            _shiftService.DeleteShift(id, institution.Id);
            
            return ResponseMessage(new HttpResponseMessage(HttpStatusCode.NoContent));
        }

        [HttpPut, Route("{id}")]
        public IHttpActionResult Update(int id, UpdateShiftDTO shiftDto)
        {
            var institution = _authManager.GetInstitutionByHeader(Request.Headers);
            if (institution == null) return BadRequest("No institution found with the given name");

            var shift = _shiftService.UpdateShift(id, institution.Id, shiftDto);

            if(shift != null)
            {
                return ResponseMessage(new HttpResponseMessage(HttpStatusCode.NoContent));
            }

            return BadRequest("The shift could not be updated!");
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

        [HttpPost, Route("createoutsideschedule"), ApiKeyFilter]
        public IHttpActionResult CreateOutsideSchedule(CreateShiftOutsideScheduleDTO shiftDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var institution = _authManager.GetInstitutionByHeader(Request.Headers);
            if (institution == null) return BadRequest("No institution found with the given name");

            var shift = _shiftService.CreateShiftOutsideSchedule(shiftDto, institution);
            if(shift != null)
            {
                return Ok(Mapper.Map(shift));
            }
            return BadRequest("Could not create shift outside of schedule!");
        }
    }
}