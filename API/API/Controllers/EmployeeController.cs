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
using System.Configuration;
using System.Linq;
using System.Web.Http.Description;
using DataTransferObjects.Manager;
using System.Web;
using System.IO;

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
        private readonly PhotoMapper _photoMapper;

        /// <summary>
        /// The constructor of the employee controller
        /// </summary>
        /// <param name="authManager"></param>
        /// <param name="employeeService"></param>
        /// <param name="photoMapper"></param>
        public EmployeeController(IAuthManager authManager, IEmployeeService employeeService, PhotoMapper photoMapper)
        {
            _authManager = authManager;
            _employeeService = employeeService;
            _photoMapper = photoMapper;
        }

        [Authorize(Roles = "Employee")]
        [HttpDelete, Route("{userId:int}/photo")]
        public IHttpActionResult DeletePhoto([FromUri] int userId)
        {
            var employee = _authManager.GetEmployeeByHeader(Request.Headers);

            // Todo: Make sure to remove old photo from database.
            _employeeService.SetPhoto(userId, employee.Organization.Id, employee.Organization.DefaultPhoto);

            return Ok();
        }

        [Authorize(Roles = "Employee")]
        [HttpPut, Route("{userId:int}/photo")]
        public IHttpActionResult UpdatePhoto([FromUri] int userId, [FromBody] string profilePhoto)
        {
            var employee = _authManager.GetEmployeeByHeader(Request.Headers);
            if (employee == null) return BadRequest("Provided token is invalid!");

            try
            {
                var photo = _photoMapper.ParseBase64Photo(profilePhoto, employee.Organization);

                _employeeService.SetPhoto(userId, employee.Organization.Id, photo);

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
        [Authorize(Roles = "Manager")]
        [HttpPost, Route("")]
        [ResponseType(typeof(EmployeeDTO))]
        public IHttpActionResult Create(CreateEmployeeDTO employeeDto)
        {
            var employee = _authManager.GetEmployeeByHeader(Request.Headers);
            if (employee == null) return BadRequest("Provided token is invalid!");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!IsCreateEmployeeDtoAlright(employeeDto))
            {
                return BadRequest("An employee must contain an email, a first name, a last name and an employee title.");
            }

            var photo = employee.Organization.DefaultPhoto;

            if (!string.IsNullOrWhiteSpace(employeeDto.ProfilePhoto))
            {
                try
                {
                    photo = _photoMapper.ParseBase64Photo(employeeDto.ProfilePhoto, employee.Organization);
                }
                catch (ArgumentOutOfRangeException ex)
                {
                    return BadRequest(ex.Message);
                }
            }

            var newEmployee = _employeeService.CreateEmployee(employeeDto, employee, photo);
            if (newEmployee != null)
            {
                return Created($"/api/employees/{newEmployee.Id}", Mapper.Map(newEmployee));
            }
            return BadRequest("The user could not be created!");
        }

        // POST api/manager/login
        /// <summary>
        /// Login as the manager with the given credentials in the body
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

        // POST api/employees/createmany
        /// <summary>
        /// Creates the employees from the content in the body.
        /// Requires 'Authorization' header set with the token granted upon manager login.
        /// </summary>
        /// <returns>
        /// Returns 'Created' (201) if the employees gets created.
        /// </returns>
        [Authorize(Roles = "Manager")]
        [HttpPost, Route("createmany")]
        [ResponseType(typeof(IEnumerable<EmployeeDTO>))]
        public IHttpActionResult CreateMany(CreateEmployeeDTO[] employeeDtos)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var employee = _authManager.GetEmployeeByHeader(Request.Headers);
            if (employee == null) return BadRequest("Provided token is invalid!");

            var employees = _employeeService.CreateManyEmployees(employeeDtos, employee);
            if (employees != null)
            {
                return Created("/api/employees", Mapper.Map(employees));
            }
            return BadRequest("The employees could not be created!");
        }

        // POST api/employees/5/resetpassword
        /// <summary>
        /// Resets the password of the employee given in the url.
        /// Requires 'Authorization' header set with the token granted upon manager login.
        /// </summary>
        /// <returns>
        /// Returns 'Ok' (200) if the employee's password gets reset.
        /// </returns>
        [Authorize(Roles = "Manager")]
        [HttpPost, Route("{id}/resetpassword")]
        public IHttpActionResult ResetPassword(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var employee = _authManager.GetEmployeeByHeader(Request.Headers);
            if (employee == null) return BadRequest("Provided token is invalid!");

            _employeeService.ResetPassword(id, employee.Organization.Id);

            return Ok(Mapper.Map("The password was reset - an email has been sent to the employee!"));
        }

        // GET api/employees
        /// <summary>
        /// Gets all the employees.
        /// Requires 'Authorization' header set with the token granted upon manager login.
        /// </summary>
        /// <returns>
        /// Returns an array of employees.
        /// </returns>
        [Authorize(Roles = "Manager, Employee")]
        [HttpGet, Route("")]
        public IHttpActionResult Get()
        {
            var employee = _authManager.GetEmployeeByHeader(Request.Headers);
            if (employee == null) return BadRequest("Provided token is invalid!");

            var employees = _employeeService.GetEmployees(employee.Organization.Id);
            if (employees == null) return NotFound();
            if (employee.Roles.Any(r => r.Name == "Manager"))
            {
                return Ok(Mapper.Map(employees));
            } else
            {
                return Ok(Mapper.MapSimple(employees));
            }

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
        [Authorize(Roles = "Manager")]
        [HttpGet, Route("{id}")]
        [ResponseType(typeof(EmployeeDTO))]
        public IHttpActionResult Get(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var employee = _authManager.GetEmployeeByHeader(Request.Headers);
            if (employee == null) return BadRequest("Provided token is invalid!");

            var foundEmployee = _employeeService.GetEmployee(id, employee.Organization.Id);
            if (foundEmployee != null)
            {
                return Ok(Mapper.Map(foundEmployee));
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
        [Authorize(Roles = "Application")]
        [HttpGet, Route("")]
        [ResponseType(typeof(IEnumerable<EmployeeDTO>))]
        public IHttpActionResult Get(string apiKey)
        {
            var institution = _authManager.GetOrganizationByApiKey(apiKey);
            if (institution == null) return BadRequest("No institution found with the given name");

            var employees = _employeeService.GetEmployees(institution.Id).OrderBy(e => e.FirstName).ThenBy(e => e.LastName);
            if (employees == null) return NotFound();
            return Ok(Mapper.Map(employees));
        }

        // PUT api/employees
        /// <summary>
        /// Updates the employee currently logged in by token.
        /// Requires 'Authorization' header set with the token granted upon manager login.
        /// </summary>
        /// <param name="employeeDto">The dto of the employee</param>
        /// <returns>
        /// Returns 'No Content' (204) if the employee gets updated.
        /// </returns>
        [Authorize(Roles = "Employee")]
        [HttpPut, Route("")]
        public IHttpActionResult Put(UpdateEmployeeDTO employeeDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var employee = _authManager.GetEmployeeByHeader(Request.Headers);
            if (employee == null) return BadRequest("Provided token is invalid!");

            Photo photo = null;

            if (!string.IsNullOrWhiteSpace(employeeDto.ProfilePhoto))
            {
                try
                {
                    photo = _photoMapper.ParseBase64Photo(employeeDto.ProfilePhoto, employee.Organization);
                }
                catch (ArgumentOutOfRangeException ex)
                {
                    return BadRequest(ex.Message);
                }
            }

            var updatedEmployee = _employeeService.UpdateEmployee(employee.Id, employeeDto, employee, photo);
            if (updatedEmployee != null)
            {
                return ResponseMessage(new HttpResponseMessage(HttpStatusCode.NoContent));
            }
            return BadRequest("Could not update the employee!");
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
        [Authorize(Roles = "Manager")]
        [HttpPut, Route("{id}")]
        public IHttpActionResult UpdateEmployee(int id, UpdateEmployeeDTO employeeDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var employee = _authManager.GetEmployeeByHeader(Request.Headers);
            if (employee == null) return BadRequest("Provided token is invalid!");

            Photo photo = null;

            if (!string.IsNullOrWhiteSpace(employeeDto.ProfilePhoto))
            {
                try
                {
                    photo = _photoMapper.ParseBase64Photo(employeeDto.ProfilePhoto, employee.Organization);
                }
                catch (ArgumentOutOfRangeException ex)
                {
                    return BadRequest(ex.Message);
                }
            }

            var updatedEmployee = _employeeService.UpdateEmployee(id, employeeDto, employee, photo);
            if (updatedEmployee != null)
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
        [Authorize(Roles = "Manager")]
        [HttpDelete, Route("{id}")]
        public IHttpActionResult Delete(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var employee = _authManager.GetEmployeeByHeader(Request.Headers);
            if (employee == null) return BadRequest("Provided token is invalid!");

            _employeeService.DeleteEmployee(id, employee);
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