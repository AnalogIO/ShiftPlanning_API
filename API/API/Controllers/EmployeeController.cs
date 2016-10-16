using API.Data;
using API.Logic;
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
        /// <summary>
        /// The controller constructor.
        /// </summary>
        public EmployeeController()
        {
            _employeeRepository = new EmployeeRepository(); 
        }

        // POST api/employees
        /// <summary>
        /// Creates the employee from the content in the body.
        /// </summary>
        [HttpPost, AdminFilter, Route("")]
        public IHttpActionResult Register(RegisterDTO employeeDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var employee = _employeeRepository.Create(employeeDto);
            if(employee != null)
            {
                return ResponseMessage(new HttpResponseMessage(HttpStatusCode.Created));
            }
            return BadRequest("A user with the given email does already exist!");
        }

        // GET api/employees
        /// <summary>
        /// Gets an array of all the employees.
        /// </summary>
        [HttpGet, AdminFilter, Route("")]
        public IHttpActionResult Get()
        {
            var employees = _employeeRepository.Read();
            return Ok(employees);
        }

        // GET api/employees/{id}
        /// <summary>
        /// Gets the employee with the given id
        /// </summary>
        [HttpGet, AdminFilter, Route("{id}")]
        public IHttpActionResult Get(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var employee = _employeeRepository.Read(id);
            return Ok(employee);
        }

        // PUT api/employees/5
        /// <summary>
        /// Updates the employee with the specified id.
        /// </summary>
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
                if(_employeeRepository.Update(EmployeeManager.UpdateEmployeeFromEmployeeDTO(employee, employeeDto)) > 0)
                {
                    return ResponseMessage(new HttpResponseMessage(HttpStatusCode.NoContent));
                }
                else
                {
                    return BadRequest("Could not update employee");
                }
            }
            return BadRequest();
        }

        // DELETE /api/employees/{id}
        /// <summary>
        /// Deletes the employee with the specified id.
        /// </summary>
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