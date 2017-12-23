using API.Logic;
using System.Web.Http;
using API.Authorization;
using Data.Services;
using DataTransferObjects.Friendship;
using System.Net.Http;
using System.Net;

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

        /// <summary>
        /// The constructor of the account controller
        /// </summary>
        /// <param name="authManager"></param>
        /// <param name="friendshipService"></param>
        public AccountController(IAuthManager authManager, IFriendshipService friendshipService)
        {
            _authManager = authManager;
            _friendshipService = friendshipService;
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
    }   
}