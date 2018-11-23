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
using System.Data;

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
        /// Requires 'Authorization' header set with the token granted upon manager login or by apikey.
        /// </summary>
        /// <returns>
        /// Returns an array of employees.
        /// </returns>
        [Authorize(Roles = "Manager, Employee, Application")]
        [HttpGet, Route("")]
        public IHttpActionResult Get()
        {
            var organization = _authManager.GetOrganizationByHeader(Request.Headers);
            if (organization == null) return BadRequest("Provided token is invalid!");

            var employees = _employeeService.GetEmployees(organization.Id);
            if (employees == null) return NotFound();
            if(_authManager.IsManager(Request.Headers))
            {
                return Ok(Mapper.Map(employees.OrderBy(e => e.FirstName).ThenBy(e => e.LastName)));
            } else
            {
                return Ok(Mapper.MapSimple(employees.OrderBy(e => e.FirstName).ThenBy(e => e.LastName)));
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

            var updatedEmployee = _employeeService.UpdateEmployee(employeeDto, employee, photo);
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

            var updateEmployee = _employeeService.GetEmployee(id, employee.Organization.Id);
            if (employee == null) throw new ObjectNotFoundException("The employee to update could not be found");

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

            var updatedEmployee = _employeeService.UpdateEmployee(employeeDto, employee, photo);
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

        [AllowAnonymous]
        [HttpGet, Route("podiosync")]
        public IHttpActionResult PodioSync()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var employees = _employeeService.SyncEmployees();
            return Ok(Mapper.Map(employees));
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