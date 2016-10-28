using API.Data;
using API.Logic;
using API.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace API.Controllers
{

    /// <summary>
    /// Controller to validate authority (login etc.)
    /// </summary>
    [RoutePrefix("api/manager")]
    public class ManagerController : ApiController
    {
        private IManagerRepository _managerRepository;

        /// <summary>
        /// The controller constructor.
        /// </summary>
        public ManagerController()
        {
            var context = new ShiftPlannerDataContext();
            _managerRepository = new ManagerRepository(context);
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