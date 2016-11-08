using API.Logic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using API.Authorization;
using DataTransferObjects;
using API.Services;

namespace API.Controllers
{
    /// <summary>
    /// Controller to manipulate with the employees.
    /// </summary>
    [RoutePrefix("api/employees")]
    public class EmployeeController : ApiController
    {
        private readonly IAuthManager _authManager;
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IAuthManager authManager, IEmployeeService employeeService)
        {
            _authManager = authManager;
            _employeeService = employeeService;
        }

        // POST api/employees
        /// <summary>
        /// Creates the employee from the content in the body.
        /// </summary>
        /// <returns>
        /// Returns 'Created' (201) if the employee gets created.
        /// If an employee already exist with the given email, the controller will return BadRequest (400).
        /// </returns>
        [HttpPost, AdminFilter, Route("")]
        public IHttpActionResult Register(CreateEmployeeDTO employeeDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var manager = _authManager.GetManagerByHeader(Request.Headers);
            if (manager == null) return BadRequest("Provided token is invalid!");

            var employee = _employeeService.CreateEmployee(employeeDto, manager);
            if (employee != null)
            {
                return ResponseMessage(new HttpResponseMessage(HttpStatusCode.Created));
            }
            return BadRequest("The user could not be created!");
        }

        // GET api/employees
        /// <summary>
        /// Gets all the employees.
        /// </summary>
        /// <returns>
        /// Returns an array of employees.
        /// </returns>
        [HttpGet, Route("")]
        public IHttpActionResult Get(int institutionId)
        {
            var employees = _employeeService.GetEmployees(institutionId);
            if (employees == null) return NotFound();
            return Ok(Mapper.Map(employees));
        }

        // GET api/employees/{id}
        /// <summary>
        /// Gets the employee with the given id.
        /// </summary>
        /// <param name="id">The id of the employee.</param>
        /// <returns>
        /// Returns the employee with the given id. 
        /// If no employee is found with the corresponding id, the controller will return NotFound (404)
        /// </returns>
        [HttpGet, Route("{id}")]
        public IHttpActionResult Get(int id, int institutionId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var employee = _employeeService.GetEmployee(id, institutionId);
            if (employee != null)
            {
                return Ok(Mapper.Map(employee));
            }
            else
            {
                return NotFound();
            }

        }

        // PUT api/employees/5
        /// <summary>
        /// Updates the employee with the specified id.
        /// </summary>
        /// <param name="id">The id of the employee.</param>
        /// <returns>
        /// Returns 'No Content' (204) if the employee gets updated.
        /// If no employee is found with the given id, the controller will return NotFound (404)
        /// </returns>
        [HttpPut, AdminFilter, Route("{id}")]
        public IHttpActionResult Put(int id, UpdateEmployeeDTO employeeDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var manager = _authManager.GetManagerByHeader(Request.Headers);
            if (manager == null) return BadRequest("Provided token is invalid!");

            var employee = _employeeService.UpdateEmployee(id, employeeDto, manager);
            if (employee != null)
            {
                return ResponseMessage(new HttpResponseMessage(HttpStatusCode.NoContent));
            }
            return BadRequest("Could not update the employee with the corresponding id!");
        }

        // DELETE /api/employees/{id}
        /// <summary>
        /// Deletes the employee with the specified id.
        /// </summary>
        /// <param name="id">The id of the employee.</param>
        /// <returns>Returns 'No Content' (204) if the employee gets deleted.</returns>
        [HttpDelete, AdminFilter, Route("{id}")]
        public IHttpActionResult Delete(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var manager = _authManager.GetManagerByHeader(Request.Headers);
            if (manager == null) return BadRequest("Provided token is invalid!");

            _employeeService.DeleteEmployee(id, manager);
            return ResponseMessage(new HttpResponseMessage(HttpStatusCode.NoContent));
        }
    }
}