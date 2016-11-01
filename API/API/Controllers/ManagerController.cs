using API.Logic;
using API.Models.DTO;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Data.Repositories;

namespace API.Controllers
{

    /// <summary>
    /// Controller to validate authority (login etc.)
    /// </summary>
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

        // POST api/manager/login
        /// <summary>
        /// Login as the manager with the given credentials in the body
        /// </summary>
        /// <returns>
        /// Returns 'Ok' (200) with a valid token if the provided username and password matches.
        /// If the provided credentials are wrong then the controller will return Unauthorized (401).
        /// </returns>
        [HttpPost, Route("login")]
        public IHttpActionResult Login(ManagerLoginDTO loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var manager = _managerRepository.Login(loginDto.Username, loginDto.Password);
            if(manager != null)
            {
                var responseDto = Mapper.ManagerToLoginResponse(manager);
                return Ok(responseDto);
            }
            HttpResponseMessage response = Request.CreateResponse<Object>(HttpStatusCode.Unauthorized, new { Message = "You entered an incorrect username or password!" });
            return ResponseMessage(response);
        }
    }
}