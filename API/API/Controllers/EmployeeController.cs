using API.Logic;
using System.Linq;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using API.Authorization;
using Data.Models;
using Data.Services;
using DataTransferObjects.Employee;
using System.Collections.Generic;
using System.Web.Http.Description;

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

        /// <summary>
        /// The constructor of the employee controller
        /// </summary>
        /// <param name="authManager"></param>
        /// <param name="employeeService"></param>
        public EmployeeController(IAuthManager authManager, IEmployeeService employeeService)
        {
            _authManager = authManager;
            _employeeService = employeeService;
        }

        [HttpPut, AdminFilter, Route("{userId:int}/photo")]
        public IHttpActionResult UpdatePhoto([FromUri] int userId)
        {
            var manager = _authManager.GetManagerByHeader(Request.Headers);
            if (manager == null) return BadRequest("Provided token is invalid!");

            var files = HttpContext.Current.Request.Files;
            
            if (files.AllKeys.Any())
            {
                var file = files[0];

                byte[] fileData = null;
                using (var binaryReader = new BinaryReader(file.InputStream))
                {
                    fileData = binaryReader.ReadBytes(file.ContentLength);
                }

                if (file.ContentType.Contains("image"))
                {
                    var photo = new Photo { Type = file.ContentType, Data = fileData };

                    _employeeService.SetPhoto(userId, manager.Organization.Id, photo);

                    return Ok();
                }

                return StatusCode(HttpStatusCode.UnsupportedMediaType);
            }

            return BadRequest();
        }

        // POST api/employees
        /// <summary>
        /// Creates the employee from the content in the body.
        /// Requires 'Authorization' header set with the token granted upon manager login.
        /// </summary>
        /// <returns>
        /// Returns 'Created' (201) if the employee gets created.
        /// If an employee already exist with the given email, the controller will return BadRequest (400).
        /// </returns>
        [HttpPost, AdminFilter, Route("")]
        [ResponseType(typeof(EmployeeDTO))]
        public IHttpActionResult Register()
        {
            var manager = _authManager.GetManagerByHeader(Request.Headers);
            if (manager == null) return BadRequest("Provided token is invalid!");

            var form = HttpContext.Current.Request.Form;

            var employeeDto = new CreateEmployeeDTO
            {
                Email = form["email"],
                FirstName = form["firstName"],
                LastName = form["lastName"],
                EmployeeTitleId = Convert.ToInt32(form["employeeTitleId"])
            };

            if (!IsCreateEmployeeDtoAlright(employeeDto))
            {
                return BadRequest("An employee must contain an email, a first name, a last name and an employee title.");
            }

            var files = HttpContext.Current.Request.Files;

            var photo = manager.Organization.DefaultPhoto;

            if (files.AllKeys.Any())
            {
                var file = files[0];

                byte[] fileData = null;
                using (var binaryReader = new BinaryReader(file.InputStream))
                {
                    fileData = binaryReader.ReadBytes(file.ContentLength);
                }

                if (file.ContentType.Contains("image"))
                {
                    photo = new Photo { Type = file.ContentType, Data = fileData };
                }
            }

            var employee = _employeeService.CreateEmployee(employeeDto, manager, photo);
            if (employee != null)
            {
                return Created($"/api/employees/{employee.Id}", Mapper.Map(employee));
            }
            return BadRequest("The user could not be created!");
        }

        // POST api/employees/createmany
        /// <summary>
        /// Creates the employees from the content in the body.
        /// Requires 'Authorization' header set with the token granted upon manager login.
        /// </summary>
        /// <returns>
        /// Returns 'Created' (201) if the employees gets created.
        /// </returns>
        [HttpPost, AdminFilter, Route("createmany")]
        [ResponseType(typeof(IEnumerable<EmployeeDTO>))]
        public IHttpActionResult RegisterMany(CreateEmployeeDTO[] employeeDtos)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var manager = _authManager.GetManagerByHeader(Request.Headers);
            if (manager == null) return BadRequest("Provided token is invalid!");

            var employees = _employeeService.CreateManyEmployees(employeeDtos, manager);
            if (employees != null)
            {
                return Created($"/api/employees", Mapper.Map(employees));
            }
            return BadRequest("The employees could not be created!");
        }

        // GET api/employees
        /// <summary>
        /// Gets all the employees.
        /// Requires 'Authorization' header set with the token granted upon manager login.
        /// </summary>
        /// <returns>
        /// Returns an array of employees.
        /// </returns>
        [HttpGet, AdminFilter, Route("")]
        [ResponseType(typeof(IEnumerable<EmployeeDTO>))]
        public IHttpActionResult Get()
        {
            var manager = _authManager.GetManagerByHeader(Request.Headers);
            if (manager == null) return BadRequest("Provided token is invalid!");

            var employees = _employeeService.GetEmployees(manager.Organization.Id);
            if (employees == null) return NotFound();
            return Ok(Mapper.Map(employees));
        }

        // GET api/employees/{id}
        /// <summary>
        /// Gets the employee with the given id.
        /// Requires 'Authorization' header set with the token granted upon manager login.
        /// </summary>
        /// <param name="id">The id of the employee.</param>
        /// <returns>
        /// Returns the employee with the given id. 
        /// If no employee is found with the corresponding id, the controller will return NotFound (404)
        /// </returns>
        [HttpGet, AdminFilter, Route("{id}")]
        [ResponseType(typeof(EmployeeDTO))]
        public IHttpActionResult Get(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var manager = _authManager.GetManagerByHeader(Request.Headers);
            if (manager == null) return BadRequest("Provided token is invalid!");

            var employee = _employeeService.GetEmployee(id, manager.Organization.Id);
            if (employee != null)
            {
                return Ok(Mapper.Map(employee));
            }
            else
            {
                return NotFound();
            }

        }

        // GET api/employees
        /// <summary>
        /// Gets all the employees.
        /// Requires 'apiKey' parameter set with the api key of the institution.
        /// </summary>
        /// <returns>
        /// Returns an array of employees.
        /// </returns>
        [HttpGet, Route("")]
        [ResponseType(typeof(IEnumerable<EmployeeDTO>))]
        public IHttpActionResult Get(string apiKey)
        {
            var institution = _authManager.GetOrganizationByApiKey(apiKey);
            if (institution == null) return BadRequest("No institution found with the given name");

            var employees = _employeeService.GetEmployees(institution.Id);
            if (employees == null) return NotFound();
            return Ok(Mapper.Map(employees));
        }

        // PUT api/employees/5
        /// <summary>
        /// Updates the employee with the specified id.
        /// Requires 'Authorization' header set with the token granted upon manager login.
        /// </summary>
        /// <param name="id">The id of the employee.</param>
        /// <param name="employeeDto">The dto of the employee</param>
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
        /// Requires 'Authorization' header set with the token granted upon manager login.
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

        private static bool IsCreateEmployeeDtoAlright(CreateEmployeeDTO dto)
        {
            if (dto == null
                || string.IsNullOrWhiteSpace(dto.Email)
                || string.IsNullOrWhiteSpace(dto.FirstName)
                || string.IsNullOrWhiteSpace(dto.LastName)) return false;
            return true;
        }
    }
}