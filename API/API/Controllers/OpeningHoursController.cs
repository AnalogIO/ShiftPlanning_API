using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using API.Mapping;
using Data.Repositories;
using DataTransferObjects.OpeningHours;

namespace API.Controllers
{
    /// <summary>
    /// The OpeningHoursController is the public entrance to get opening hour and shift information.
    /// </summary>
    [RoutePrefix("api/{shortKey}/openinghours")]
    public class OpeningHoursController : ApiController
    {
        private readonly IShiftRepository _shiftRepository;
        private readonly IInstitutionRepository _institutionRepository;
        private readonly IOpeningHoursMapper _mapper;

        public OpeningHoursController(IShiftRepository shiftRepository, IInstitutionRepository institutionRepository, IOpeningHoursMapper mapper)
        {
            _shiftRepository = shiftRepository;
            _institutionRepository = institutionRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Fetch shifts for the next week for a given institution.
        /// </summary>
        /// <param name="shortKey">ShortKey of the institution to fetch opening hours for.</param>
        /// <returns></returns>
        [Route("")]
        [ResponseType(typeof(IEnumerable<OpeningHoursDTO>))]
        public IHttpActionResult Get(string shortKey)
        {
            if (!_institutionRepository.Exists(shortKey))
            {
                return NotFound();
            }

            DateTime today = DateTime.Today, inOneWeek = today.AddDays(7);

            return Ok(
                _mapper.MapToDto(_shiftRepository
                    .ReadFromInstitution(shortKey)
                    .Where(shift => shift.Start >= today && shift.End <= inOneWeek)
                )
            );
        }
    }
}
