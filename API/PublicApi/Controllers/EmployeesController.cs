using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Description;
using Data.Services;
using DataTransferObjects.Public.Employees;
using PublicApi.Mapping;

namespace PublicApi.Controllers
{
    /// <summary>
    /// The public API to access employee information for organizations.
    /// </summary>
    [RoutePrefix("api/employees")]
    public class EmployeesController : ApiController
    {
        private readonly IEmployeeService _employeeService;
        private readonly IVolunteerMapper _volunteerMapper;

        public EmployeesController(IEmployeeService employeeService, IVolunteerMapper volunteerMapper)
        {
            _employeeService = employeeService;
            _volunteerMapper = volunteerMapper;
        }

        /// <summary>
        /// Retrive all employees for a given organization.
        /// </summary>
        /// <param name="shortKey">The shortkey of the organization.</param>
        /// <returns>A collection of employees, if the organization was found. Http 404 otherwise.</returns>
        [HttpGet, Route("{shortKey}")]
        [ResponseType(typeof(IEnumerable<EmployeeDTO>))]
        public IHttpActionResult Get(string shortKey, bool active = true)
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
        [ResponseType(typeof(EmployeeDTO))]
        public IHttpActionResult Get(string shortKey, int id)
        {
            var employee = _employeeService.GetEmployee(id, shortKey);

            if (employee == null)
                return NotFound();

            return Ok(_volunteerMapper.Map(employee));
        }
        
    }
}