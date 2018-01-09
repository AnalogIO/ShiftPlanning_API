using API.Logic;
using System.Web.Http;
using API.Authorization;
using Data.Services;
using DataTransferObjects.Friendship;
using System.Net.Http;
using System.Net;
using DataTransferObjects.Employee;
using System.Configuration;
using System.Linq;
using Data.Models;
using System;

namespace API.Controllers
{
    /// <summary>
    /// Controller to manipulate with the logged in account.
    /// </summary>
    [RoutePrefix("api/account")]
    public class AccountController : ApiController
    {
        private readonly IAuthManager _authManager;
        private readonly IFriendshipService _friendshipService;
        private readonly IEmployeeService _employeeService;
        private readonly PhotoMapper _photoMapper;

        /// <summary>
        /// The constructor of the account controller
        /// </summary>
        /// <param name="authManager"></param>
        /// <param name="friendshipService"></param>
        /// <param name="ëmployeeService"></param>
        public AccountController(IAuthManager authManager, IFriendshipService friendshipService, IEmployeeService employeeService, PhotoMapper photoMapper)
        {
            _authManager = authManager;
            _friendshipService = friendshipService;
            _employeeService = employeeService;
            _photoMapper = photoMapper;
        }

        [Authorize(Roles = "Employee")]
        [HttpPost, Route("friendships")]
        public IHttpActionResult CreateFriendship([FromBody] CreateFriendshipDTO dto)
        {
            var employee = _authManager.GetEmployeeByHeader(Request.Headers);

            var friendship = _friendshipService.CreateFriendship(employee, dto.FriendEmployeeId);
            if (friendship == null) return BadRequest("Friendship could not be created");

            return Created($"/api/account/friendships/{friendship.Id}", Mapper.Map(friendship));
        }

        [Authorize(Roles = "Employee")]
        [HttpDelete, Route("friendships/{friendshipId}")]
        public IHttpActionResult DeleteFriendship(int friendshipId)
        {
            var employee = _authManager.GetEmployeeByHeader(Request.Headers);

            _friendshipService.DeleteFriendship(employee, friendshipId);

            return ResponseMessage(new HttpResponseMessage(HttpStatusCode.NoContent));
        }

        [Authorize(Roles = "Employee")]
        [HttpGet, Route("friendships")]
        public IHttpActionResult GetFriendships()
        {
            var employee = _authManager.GetEmployeeByHeader(Request.Headers);

            var friendships = _friendshipService.GetFriendships(employee);

            return Ok(Mapper.Map(friendships));
        }

        // PUT api/account
        /// <summary>
        /// Update the account with the given token in the `authorization` header
        /// If `OldPassword` and `NewPassword` is specified, the service will update the password if `OldPassword` matches the current password and if the length of `NewPassword` is minimum 8.
        /// </summary>
        /// <returns>
        /// Returns 'NoContent' (204) if the update succeeds.
        /// If the provided `Authorization` token is invalid the controller will return Unauthorized (401).
        /// </returns>
        [Authorize(Roles = "Employee")]
        [HttpPut, Route("")]
        public IHttpActionResult Update([FromBody] UpdateEmployeeDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var employee = _authManager.GetEmployeeByHeader(Request.Headers);
            if (employee == null) return BadRequest("Provided token is invalid!");

            Photo photo = null;

            if (!string.IsNullOrWhiteSpace(dto.ProfilePhoto))
            {
                try
                {
                    photo = _photoMapper.ParseBase64Photo(dto.ProfilePhoto, employee.Organization);
                }
                catch (ArgumentOutOfRangeException ex)
                {
                    return BadRequest(ex.Message);
                }
            }

            var updatedEmployee = _employeeService.UpdateEmployee(dto, employee, photo);
            if (updatedEmployee != null)
            {
                return ResponseMessage(new HttpResponseMessage(HttpStatusCode.NoContent));
            }
            return BadRequest("Could not update the employee!");
        }

        // POST api/account/login
        /// <summary>
        /// Login as the employee with the given credentials in the body
        /// </summary>
        /// <returns>
        /// Returns 'Ok' (200) with a valid token if the provided username and password matches.
        /// If the provided credentials are wrong then the controller will return Unauthorized (401).
        /// </returns>
        [AllowAnonymous]
        [HttpPost, Route("login")]
        public IHttpActionResult Login(EmployeeLoginDTO loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var employee = _employeeService.Login(loginDto.Username.Trim(), loginDto.Password);
            if (employee != null)
            {
                var responseDto = new EmployeeLoginResponse
                {
                    Token = employee.Tokens.LastOrDefault()?.TokenHash,
                    OrganizationId = employee.Organization.Id,
                    OrganizationName = employee.Organization.Name,
                    Expires = int.Parse(ConfigurationManager.AppSettings["TokenAgeHour"]) * 60 * 60, // from hours to seconds 
                    Employee = Mapper.Map(employee)
                };
                return Ok(responseDto);
            }

            HttpResponseMessage response = Request.CreateResponse<object>(HttpStatusCode.Unauthorized, new { Message = "You entered an incorrect username or password!" });
            return ResponseMessage(response);
        }
    }   
}