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
    /// The public API to access employee information for institutions.
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
        /// Retrive all employees for a given institution.
        /// </summary>
        /// <param name="shortKey">The shortkey of the institution.</param>
        /// <returns>A collection of employees, if the institution was found. Http 404 otherwise.</returns>
        [HttpGet, Route("{shortKey}")]
        [ResponseType(typeof(IEnumerable<EmployeeDTO>))]
        public IHttpActionResult Get(string shortKey)
        {
            var employees = _employeeService.GetEmployees(shortKey)?.ToList();

            if (employees == null)
            {
                return NotFound();
            }

            return Ok(_volunteerMapper.Map(employees));
        }

        /// <summary>
        /// Retrieve a certain employee from an institution.
        /// </summary>
        /// <param name="shortKey">The shortkey of an institution.</param>
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

        /// <summary>
        /// Retrieve the photo for an employee.
        /// </summary>
        /// <param name="shortKey">The shortkey of an institution.</param>
        /// <param name="id">The id of an employee.</param>
        /// <returns>
        /// A photo if the shortkey/id match was found. Http 404 otherwise.
        /// If the employee has no photo registered, an empty response is generated.
        /// </returns>
        [HttpGet, Route("{shortKey}/{id}/image")]
        public IHttpActionResult GetImage(string shortKey, int id)
        {
            var employee = _employeeService.GetEmployee(id, shortKey);

            if (employee == null)
                return NotFound();

            var message = new HttpResponseMessage(HttpStatusCode.OK);

            if (employee.Photo != null)
            {
                message.Content = new ByteArrayContent(employee.Photo.Data);
                message.Content.Headers.ContentType = MediaTypeHeaderValue.Parse(employee.Photo.Type);
            }
            else
            {
                message.Content = new ByteArrayContent(new byte[0]); // TODO: What should an empty response be?
            }

            return ResponseMessage(message);
        }
    }
}