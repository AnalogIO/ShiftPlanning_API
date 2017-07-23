using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Data.Repositories;
using System.Linq;
using API.Authorization;
using DataTransferObjects.Manager;

namespace API.Controllers
{

    /// <summary>
    /// Controller to validate authority (login etc.)
    /// </summary>
    [Authorize(Roles = "Manager")]
    [RoutePrefix("api/manager")]
    public class ManagerController : ApiController
    {
        private readonly IManagerRepository _managerRepository;

        /// <summary>
        /// The controller constructor.
        /// </summary>
        public ManagerController(IManagerRepository managerRepository)
        {
            _managerRepository = managerRepository;
        }

        // POST api/manager/validate
        /// <summary>
        /// Validates the token set in the 'Authorization' header.
        /// </summary>
        /// <returns>
        /// Returns 'Ok' (200) if the token is valid.
        /// If the token is invalid then the controller will return Unauthorized (401).
        /// </returns>
        [HttpPost, Route("validate")]
        public IHttpActionResult Validate()
        {
            return Ok();
        }
    }
}