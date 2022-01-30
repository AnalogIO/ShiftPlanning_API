using Microsoft.AspNetCore.Mvc;
using ShiftPlanning.DTOs.Public.OpeningHours;
using ShiftPlanning.WebApi.Services;

namespace ShiftPlanning.WebApi.Controllers
{
    /// <summary>
    /// The OpenController is the public interface that can tell whether or not
    /// a given organization is open.
    /// </summary>
    [ApiController]
    [Route("api/open")]
    public class OpenController : ControllerBase
    {
        private readonly IShiftService _shiftService;

        /// <summary>
        /// Dependency injection constructor of OpenController
        /// </summary>
        /// <param name="shiftService">An IShiftService implementation</param>
        public OpenController(IShiftService shiftService)
        {
            _shiftService = shiftService;
        }

        /// <summary>
        /// Find out whether or not the organization identified by short key is open based on whether or not any employee has checked in.
        /// </summary>
        /// <param name="shortKey">The short key of the institution.</param>
        /// <returns></returns>
        [ProducesResponseType(typeof(IsOpenDTO), 200)]
        [HttpGet, Route("{shortKey}")]
        public IActionResult IsOpen(string shortKey)
        {
            var isOpen = new IsOpenDTO
            {
                Open = _shiftService.IsOrganisationOpen(shortKey)
            };

            return Ok(isOpen);
        }
    }
}