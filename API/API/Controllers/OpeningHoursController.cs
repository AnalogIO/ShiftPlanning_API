using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;
using API.Mapping;
using API.Services;
using DataTransferObjects.OpeningHours;

namespace API.Controllers
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
    }
}
