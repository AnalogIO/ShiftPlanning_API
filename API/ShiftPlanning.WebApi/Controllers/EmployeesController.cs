using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShiftPlanning.DTOs.Employee;
using ShiftPlanning.Model.Models;
using ShiftPlanning.WebApi.Exceptions;
using ShiftPlanning.WebApi.Helpers.Authorization;
using ShiftPlanning.WebApi.Helpers.Mappers;
using ShiftPlanning.WebApi.Services;

namespace ShiftPlanning.WebApi.Controllers
{
    /// <summary>
    /// The public API to access employee information for organizations.
    /// </summary>
    [ApiController]
    [Route("api/employees")]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        private readonly IVolunteerMapper _volunteerMapper;
        private readonly IAuthManager _authManager;
        private readonly IPhotoMapper _photoMapper;

        public EmployeesController(IEmployeeService employeeService, IVolunteerMapper volunteerMapper, IAuthManager authManager, IPhotoMapper photoMapper)
        {
            _employeeService = employeeService;
            _volunteerMapper = volunteerMapper;
            _authManager = authManager;
            _photoMapper = photoMapper;
        }

        /// <summary>
        /// Retrive all employees for a given organization.
        /// </summary>
        /// <param name="shortKey">The shortkey of the organization.</param>
        /// <returns>A collection of employees, if the organization was found. Http 404 otherwise.</returns>
        [HttpGet, Route("{shortKey}")]
        [ProducesResponseType(typeof(IEnumerable<EmployeeDTO>), 200)]
        public IActionResult Get(string shortKey, bool active = true)
        {
            var employees = _employeeService.GetEmployeesByActivity(shortKey, active)?.ToList();

            if (employees == null)
            {
                return NotFound();
            }

            return Ok(_volunteerMapper.Map(employees.OrderBy(x => x.FirstName).ThenBy(x => x.LastName)));
        }

        /// <summary>
        /// Retrieve a certain employee from an organization.
        /// </summary>
        /// <param name="shortKey">The shortkey of an organization.</param>
        /// <param name="id">The id of the employee.</param>
        /// <returns>A representation of the employee, if a shortKey/id match was found.</returns>
        [HttpGet, Route("{shortKey}/{id}")]
        [ProducesResponseType(typeof(EmployeeDTO),200)]
        public IActionResult Get(string shortKey, int id)
        {
            var employee = _employeeService.GetEmployee(id, shortKey);

            if (employee == null)
                return NotFound();

            return Ok(_volunteerMapper.Map(employee));
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
        [ProducesResponseType(typeof(EmployeeDTO), 201)]
        public IActionResult Create(CreateEmployeeDTO employeeDto)
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

            var newEmployee = _employeeService.CreateEmployee(employeeDto, employee);
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
        [ProducesResponseType(typeof(IEnumerable<EmployeeDTO>), 201)]
        public IActionResult CreateMany(CreateEmployeeDTO[] employeeDtos)
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
        public IActionResult ResetPassword(int id)
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
        public IActionResult Get()
        {
            var organization = _authManager.GetOrganizationByHeader(Request.Headers);
            if (organization == null) return BadRequest("Provided token is invalid!");

            var employees = _employeeService.GetEmployees(organization.Id);
            if (employees == null) return NotFound();
            //get claims of the Role type
            if(User.IsInRole("Manager"))
            {
                return Ok(Mapper.Map(employees.OrderBy(e => e.FirstName).ThenBy(e => e.LastName)));
            }

            return Ok(Mapper.MapSimple(employees.OrderBy(e => e.FirstName).ThenBy(e => e.LastName)));

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
        [HttpGet, Route("{id:int}")]
        [ProducesResponseType(typeof(EmployeeDTO), 200)]
        public IActionResult Get(int id)
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

            return NotFound();

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
        public IActionResult Put(UpdateEmployeeDTO employeeDto)
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
                return NoContent();
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
        public IActionResult UpdateEmployee(int id, UpdateEmployeeDTO employeeDto)
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
                return NoContent();
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
        public IActionResult Delete(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var employee = _authManager.GetEmployeeByHeader(Request.Headers);
            if (employee == null) return BadRequest("Provided token is invalid!");

            _employeeService.DeleteEmployee(id, employee);
            return NoContent();
        }

        [AllowAnonymous]
        [HttpGet, Route("podiosync")]
        public IActionResult PodioSync(string shortKey = "analog")
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedCount = _employeeService.SyncEmployees(shortKey);
            return Ok(new { SyncCount = updatedCount });
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