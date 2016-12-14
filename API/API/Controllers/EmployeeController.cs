using API.Logic;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using API.Authorization;
using Data.Models;
using Data.Services;
using DataTransferObjects.Employee;
using System.Collections.Generic;
using System.Text.RegularExpressions;
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

        [HttpDelete, AdminFilter, Route("{userId:int}/photo")]
        public IHttpActionResult DeletePhoto([FromUri] int userId)
        {
            var manager = _authManager.GetManagerByHeader(Request.Headers);
            if (manager == null) return BadRequest("Provided token is invalid!");

            // Todo: Make sure to remove old photo from database.
            _employeeService.SetPhoto(userId, manager.Organization.Id, manager.Organization.DefaultPhoto);

            return Ok();
        }

        [HttpPut, AdminFilter, Route("{userId:int}/photo")]
        public IHttpActionResult UpdatePhoto([FromUri] int userId, [FromBody] string profilePhoto)
        {
            var manager = _authManager.GetManagerByHeader(Request.Headers);
            if (manager == null) return BadRequest("Provided token is invalid!");

            try
            {
                var photo = ParseBase64Photo(profilePhoto, manager.Organization);

                _employeeService.SetPhoto(userId, manager.Organization.Id, photo);

                return Ok();
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return BadRequest(ex.Message);
            }
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
        public IHttpActionResult Register(CreateEmployeeDTO employeeDto)
        {
            var manager = _authManager.GetManagerByHeader(Request.Headers);
            if (manager == null) return BadRequest("Provided token is invalid!");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!IsCreateEmployeeDtoAlright(employeeDto))
            {
                return BadRequest("An employee must contain an email, a first name, a last name and an employee title.");
            }
            
            var photo = manager.Organization.DefaultPhoto;

            if (!string.IsNullOrWhiteSpace(employeeDto.ProfilePhoto))
            {
                try
                {
                    photo = ParseBase64Photo(employeeDto.ProfilePhoto, manager.Organization);
                }
                catch (ArgumentOutOfRangeException ex)
                {
                    return BadRequest(ex.Message);
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

            Photo photo = null;

            if (!string.IsNullOrWhiteSpace(employeeDto.ProfilePhoto))
            {
                try
                {
                    photo = ParseBase64Photo(employeeDto.ProfilePhoto, manager.Organization);
                }
                catch (ArgumentOutOfRangeException ex)
                {
                    return BadRequest(ex.Message);
                }
            }

            var employee = _employeeService.UpdateEmployee(id, employeeDto, manager, photo);
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="base64EncodedPhoto"></param>
        /// <param name="organization"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException">If the encoded string does not contain a media type.</exception>
        private Photo ParseBase64Photo(string base64EncodedPhoto, Organization organization)
        {
            const string pattern = @"^data:([a-z/]+);base64,";
            var match = Regex.Match(base64EncodedPhoto, pattern);

            if (!match.Success)
            {
                throw new ArgumentOutOfRangeException(nameof(base64EncodedPhoto), "Unsupported image type");
            }
            
            var type = match.Groups[0].Value;

            var data = Convert.FromBase64String(Regex.Replace(base64EncodedPhoto, pattern, ""));

            return new Photo
            {
                Data = data,
                Type = type,
                Organization = organization
            };
        }
    }
}