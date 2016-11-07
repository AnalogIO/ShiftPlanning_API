using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using API.Services;
using DataTransferObjects.OpeningHours;

namespace API.Controllers
{
    /// <summary>
    /// The IsOpenController is the public interface that can tell whether or not
    /// a given institution is open.
    /// </summary>
    [RoutePrefix("api/{shortKey}/isOpen")]
    public class IsOpenController : ApiController
    {
        private readonly IShiftService _shiftService;

        /// <summary>
        /// Dependency injection constructor of IsOpenController
        /// </summary>
        /// <param name="shiftService">An IShiftService implementation</param>
        public IsOpenController(IShiftService shiftService)
        {
            _shiftService = shiftService;
        }

        /// <summary>
        /// Find out whether or not the institution identified by short key is open.
        /// </summary>
        /// <param name="shortKey">The short key of the institution.</param>
        /// <returns></returns>
        [ResponseType(typeof(IsOpenDTO))]
        [HttpGet, Route("")]
        public IHttpActionResult IsOpen(string shortKey)
        {
            var currentShifts = _shiftService.GetOngoingShiftsByInstitution(shortKey);
            
            if (currentShifts == null)
            {
                return NotFound();
            }
            
            return Ok(new IsOpenDTO
            {
                Open = currentShifts.Any()
            });
        }
    }
}
