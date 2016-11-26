using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using Data.Services;
using DataTransferObjects.OpeningHours;
using PublicApi.Mapping;

namespace PublicApi.Controllers
{
    /// <summary>
    /// The OpeningHoursController is the public entrance to get opening hour and shift information.
    /// </summary>
    [RoutePrefix("api/{shortKey}/openinghours")]
    public class OpeningHoursController : ApiController
    {
        private readonly IShiftService _shiftService;
        private readonly IOpeningHoursMapper _mapper;

        /// <summary>
        /// Dependency injection constructor.
        /// </summary>
        /// <param name="shiftService"></param>
        /// <param name="mapper"></param>
        public OpeningHoursController(IShiftService shiftService, IOpeningHoursMapper mapper)
        {
            _shiftService = shiftService;
            _mapper = mapper;
        }

        /// <summary>
        /// Fetch shifts for the next week for a given institution.
        /// </summary>
        /// <param name="shortKey">ShortKey of the institution to fetch opening hours for.</param>
        /// <returns>A collection of OpeningHoursDTO. NotFound if institutionRepository was not found.</returns>
        [HttpGet, Route("")]
        [ResponseType(typeof(IEnumerable<OpeningHoursDTO>))]
        public IHttpActionResult Get(string shortKey)
        {
            var shifts = _shiftService.GetByInstitution(shortKey, DateTime.Today, DateTime.Today.AddDays(7));

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
        [HttpGet, Route("intervals/{interval:int}")]
        [ResponseType(typeof(IEnumerable<OpeningHoursDTO>))]
        public IHttpActionResult GetIntervals(string shortKey, int interval)
        {
            var shifts = _shiftService.GetByInstitution(shortKey, DateTime.Today, DateTime.Today.AddDays(7))?.ToList();

            if (shifts == null) return NotFound();

            return Ok(_mapper.MapToIntervalDto(shifts, interval));
        }
    }
}