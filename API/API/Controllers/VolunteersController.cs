using API.Mapping;
using API.Services;
using DataTransferObjects.Volunteers;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Description;

namespace API.Controllers
{
    [RoutePrefix("api/{shortKey}/volunteers")]
    public class VolunteersController : ApiController
    {
        private readonly IEmployeeService _employeeService;
        private readonly IVolunteerMapper _volunteerMapper;

        public VolunteersController(IEmployeeService employeeService, IVolunteerMapper volunteerMapper)
        {
            _employeeService = employeeService;
            _volunteerMapper = volunteerMapper;
        }


        [HttpGet, Route("")]
        [ResponseType(typeof(IEnumerable<VolunteerDTO>))]
        public IHttpActionResult Get(string shortKey)
        {
            var employees = _employeeService.GetEmployees(shortKey)?.ToList();

            if (employees == null)
            {
                return NotFound();
            }

            return Ok(_volunteerMapper.Map(employees));
        }

        [HttpGet, Route("{id}")]
        [ResponseType(typeof(VolunteerDTO))]
        public IHttpActionResult Get(string shortKey, int id)
        {
            var employee = _employeeService.GetEmployee(id, shortKey);

            if (employee == null)
                return NotFound();

            return Ok(_volunteerMapper.Map(employee));
        }

        [HttpGet, Route("{id}/image")]
        //[ResponseType(typeof(VolunteerDTO))]
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