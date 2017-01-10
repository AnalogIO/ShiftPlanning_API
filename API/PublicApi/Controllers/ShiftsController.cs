using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using Data.Services;
using DataTransferObjects.Public.OpeningHours;
using PublicApi.Mapping;

namespace PublicApi.Controllers
{
    /// <summary>
    /// The ShiftsController is the public entrance to get opening hour and shift information.
    /// </summary>
    [RoutePrefix("api/shifts")]
    public class ShiftsController : ApiController
    {
        private readonly IShiftService _shiftService;
        private readonly IOpeningHoursMapper _mapper;

        /// <summary>
        /// Dependency injection constructor.
        /// </summary>
        /// <param name="shiftService"></param>
        /// <param name="mapper"></param>
        public ShiftsController(IShiftService shiftService, IOpeningHoursMapper mapper)
        {
            _shiftService = shiftService;
            _mapper = mapper;
        }

        /// <summary>
        /// Fetch shifts for the next week for a given institution.
        /// </summary>
        /// <param name="shortKey">ShortKey of the institution to fetch opening hours for.</param>
        /// <returns>A collection of OpeningHoursDTO. NotFound if institutionRepository was not found.</returns>
        [HttpGet, Route("{shortKey}")]
        [ResponseType(typeof(IEnumerable<OpeningHoursDTO>))]
        public IHttpActionResult Get(string shortKey)
        {
            var monday = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Monday);
            var shifts = _shiftService.GetByOrganization(shortKey, monday, monday.AddDays(7));

            if (shifts == null)
            {
                return NotFound();
            }

            return Ok(_mapper.MapToDto(shifts));
        }

        /// <summary>
        /// Fetch shifts for the next week for a given institution.
        /// </summary>
        /// <param name="shortKey">ShortKey of the institution to fetch opening hours for.</param>
        /// <param name="interval">The number of minutes an interval should span.</param>
        /// <returns>A collection of OpeningHoursDTO. NotFound if institutionRepository was not found.</returns>
        [HttpGet, Route("~/api/openinghours/{shortKey}")]
        [ResponseType(typeof(IEnumerable<OpeningHoursDTO>))]
        public IHttpActionResult GetIntervals(string shortKey, int interval = 30)
        {
            var shifts = _shiftService.GetByOrganization(shortKey, DateTime.Today, DateTime.Today.AddDays(7))?.ToList();

            if (shifts == null) return NotFound();

            return Ok(_mapper.MapToIntervalDto(shifts, interval));
        }
    }
}