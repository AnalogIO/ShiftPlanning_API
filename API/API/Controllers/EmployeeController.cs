using API.Data;
using API.Logic;
using API.Models;
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
    /// <summary>
    /// Controller to manipulate with the employees.
    /// </summary>
    [RoutePrefix("api/employees")]
    public class EmployeeController : ApiController
    {
        private IEmployeeRepository _employeeRepository;
        private IEmployeeTitleRepository _employeeTitleRepository;
        
        /// <summary>
        /// The controller constructor.
        /// </summary>
        public EmployeeController()
        {
            var context = new ShiftPlannerDataContext();
            _employeeRepository = new EmployeeRepository(context);
            _employeeTitleRepository = new EmployeeTitleRepository(context);
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

            var employee = new Employee { Email = employeeDto.Email, FirstName = employeeDto.FirstName, LastName = employeeDto.LastName };
            var title = _employeeTitleRepository.Read(employeeDto.EmployeeTitleId);
            if (title != null) employee.EmployeeTitle = title;
            employee = _employeeRepository.Create(employee);
            if(employee != null)
            {
                return ResponseMessage(new HttpResponseMessage(HttpStatusCode.Created));
            }
            return BadRequest("A user with the given email does already exist!");
        }

        // GET api/employees
        /// <summary>
        /// Gets all the employees.
        /// </summary>
        /// <returns>
        /// Returns an array of employees.
        /// </returns>
        [HttpGet, AdminFilter, Route("")]
        public IHttpActionResult Get()
        {
            var employees = _employeeRepository.Read();
            return Ok(employees);
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
        [HttpGet, AdminFilter, Route("{id}")]
        public IHttpActionResult Get(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var employee = _employeeRepository.Read(id);
            if(employee != null)
            {
                return Ok(employee);
            } else
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

            var employee = _employeeRepository.Read(id);
            if(employee != null)
            {
                employee = EmployeeManager.UpdateEmployeeFromEmployeeDTO(employee, employeeDto);
                var title = _employeeTitleRepository.Read(employeeDto.EmployeeTitleId);
                if (title != null) employee.EmployeeTitle = title;
                if (_employeeRepository.Update(employee) > 0)
                {
                    return ResponseMessage(new HttpResponseMessage(HttpStatusCode.NoContent));
                }
                else
                {
                    return BadRequest("Could not update employee");
                }
            }
            return NotFound();
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

            _employeeRepository.Delete(id);
            return ResponseMessage(new HttpResponseMessage(HttpStatusCode.NoContent));
        }
    }
}