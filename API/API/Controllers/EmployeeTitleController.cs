using System.Net;
using System.Net.Http;
using System.Web.Http;
using API.Authorization;
using API.Services;
using API.Logic;
using DataTransferObjects.EmployeeTitles;

namespace API.Controllers
{
    /// <summary>
    /// Controller to manipulate with the employee titles.
    /// </summary>
    [RoutePrefix("api/employeetitles")]
    public class EmployeeTitleController : ApiController
    {
        private readonly IAuthManager _authManager;
        private readonly IEmployeeTitleService _employeeTitleService;

        public EmployeeTitleController(IAuthManager authManager, IEmployeeTitleService employeeTitleService)
        {
            _authManager = authManager;
            _employeeTitleService = employeeTitleService;
        }

        // POST api/employeetitles
        /// <summary>
        /// Creates the employee title from the content in the body.
        /// Requires 'Authorization' header set with the token granted upon manager login.
        /// </summary>
        /// <returns>
        /// Returns 'Created' (201) if the employee title gets created.
        /// </returns>
        [HttpPost, AdminFilter, Route("")]
        public IHttpActionResult Register(CreateEmployeeTitleDTO employeeTitleDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var token = Request.Headers.Authorization.ToString();

            var manager = _authManager.GetManagerByHeader(Request.Headers);
            if (manager == null) return BadRequest("Provided token is invalid!");

            var employeeTitle = _employeeTitleService.CreateEmployeeTitle(employeeTitleDto, manager);
            if (employeeTitle != null)
            {
                return ResponseMessage(new HttpResponseMessage(HttpStatusCode.Created));
            }
            return BadRequest("Could not create the employee title!");
        }


        // GET api/employeetitles
        /// <summary>
        /// Gets all the employee titles.
        /// Requires 'Authorization' header set with the token granted upon manager login.
        /// </summary>
        /// <returns>
        /// Returns an array of employee titles.
        /// </returns>
        [HttpGet, AdminFilter, Route("")]
        public IHttpActionResult Get()
        {
            var manager = _authManager.GetManagerByHeader(Request.Headers);
            if (manager == null) return BadRequest("Provided token is invalid!");

            var employeeTitles = _employeeTitleService.GetEmployeeTitles(manager);
            return Ok(Mapper.Map(employeeTitles));
        }


        // GET api/employeetitles/{id}
        /// <summary>
        /// Gets the employee title with the given id.
        /// Requires 'Authorization' header set with the token granted upon manager login.
        /// </summary>
        /// <param name="id">The id of the employee title.</param>
        /// <returns>
        /// Returns the employee title with the given id. 
        /// If no employee title is found with the corresponding id, the controller will return NotFound (404)
        /// </returns>
        [HttpGet, AdminFilter, Route("{id}")]
        public IHttpActionResult Get(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var manager = _authManager.GetManagerByHeader(Request.Headers);
            if (manager == null) return BadRequest("Provided token is invalid!");

            var employeeTitle = _employeeTitleService.GetEmployeeTitles(manager);
            if (employeeTitle != null)
            {
                return Ok(Mapper.Map(employeeTitle));
            }
            else
            {
                return NotFound();
            }

        }


        // PUT api/employeetitles/5
        /// <summary>
        /// Updates the employee title with the specified id.
        /// Requires 'Authorization' header set with the token granted upon manager login.
        /// </summary>
        /// <param name="id">The id of the employee title.</param>
        /// <returns>
        /// Returns 'No Content' (204) if the employee title gets updated.
        /// If no employee title is found with the given id, the controller will return NotFound (404)
        /// </returns>
        [HttpPut, AdminFilter, Route("{id}")]
        public IHttpActionResult Put(int id, UpdateEmployeeTitleDTO employeeTitleDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var manager = _authManager.GetManagerByHeader(Request.Headers);
            if (manager == null) return BadRequest("Provided token is invalid!");

            var employeeTitle = _employeeTitleService.UpdateEmployeeTitle(id, employeeTitleDto, manager);
            if (employeeTitle != null)
            {
                    return ResponseMessage(new HttpResponseMessage(HttpStatusCode.NoContent));
            }

            return BadRequest("Could not update employee title");
        }


        // DELETE /api/employeetitles/{id}
        /// <summary>
        /// Deletes the employee title with the specified id.
        /// Requires 'Authorization' header set with the token granted upon manager login.
        /// </summary>
        /// <param name="id">The id of the employee title.</param>
        /// <returns>Returns 'No Content' (204) if the employee title gets deleted.</returns>
        [HttpDelete, AdminFilter, Route("{id}")]
        public IHttpActionResult Delete(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var manager = _authManager.GetManagerByHeader(Request.Headers);
            if (manager == null) return BadRequest("Provided token is invalid!");

            _employeeTitleService.DeleteEmployeeTitle(id, manager);
            return ResponseMessage(new HttpResponseMessage(HttpStatusCode.NoContent));
        }

    }
}