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
    [RoutePrefix("api/users")]
    public class UsersController : ApiController
    {
        private IUserRepository _userRepository;
        public UsersController()
        {
            _userRepository = new UserRepository(); 
        }

        // POST api/users
        [HttpPost, AdminFilter]
        public IHttpActionResult Register(RegisterDTO userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = _userRepository.Create(userDto);
            if(user != null)
            {
                return ResponseMessage(new HttpResponseMessage(HttpStatusCode.Created));
            }
            return BadRequest("A user with the given email does already exist!");
        }
    }
}