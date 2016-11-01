using API.Logic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Data.Models;
using Data.Repositories;
using DataTransferObjects;

namespace API.Controllers
{
    /// <summary>
    /// Controller to manipulate with the employee titles.
    /// </summary>
    [RoutePrefix("api/employeetitles")]
    public class EmployeeTitleController : ApiController
    {
        private readonly IEmployeeTitleRepository _employeeTitleRepository;
        private readonly IManagerRepository _managerRepository;

        public EmployeeTitleController(IEmployeeTitleRepository employeeTitleRepository, 
            IManagerRepository managerRepository)
        {
            _employeeTitleRepository = employeeTitleRepository;
            _managerRepository = managerRepository;
        }

        // POST api/employeetitles
        /// <summary>
        /// Creates the employee title from the content in the body.
        /// </summary>
        /// <returns>
        /// Returns 'Created' (201) if the employee title gets created.
        /// </returns>
        [HttpPost, AdminFilter, Route("")]
        public IHttpActionResult Register(CreateEmployeeTitleDTO employeeTitleDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var token = Request.Headers.Authorization.ToString();

            var manager = _managerRepository.Read(token);
            if (manager == null) return BadRequest("Provided token is invalid!");

            var employeeTitle = new EmployeeTitle { Title = employeeTitleDto.Title, Institution = manager.Institution };
            employeeTitle = _employeeTitleRepository.Create(employeeTitle);
            if (employeeTitle != null)
            {
                return ResponseMessage(new HttpResponseMessage(HttpStatusCode.Created));
            }
            return BadRequest("Could not create the employee title!");
        }


        // GET api/employeetitles
        /// <summary>
        /// Gets all the employee titles.
        /// </summary>
        /// <returns>
        /// Returns an array of employee titles.
        /// </returns>
        [HttpGet, AdminFilter, Route("")]
        public IHttpActionResult Get()
        {
            var manager = GetManager();
            if (manager == null) return BadRequest("Provided token is invalid!");

            var employeeTitles = _employeeTitleRepository.ReadFromInstitution(manager.Institution.Id);
            return Ok(employeeTitles);
        }


        // GET api/employeetitles/{id}
        /// <summary>
        /// Gets the employee title with the given id.
        /// </summary>
        /// <param name="id">The id of the employee title.</param>
        /// <returns>
        /// Returns the employee title with the given id. 
        /// If no employee title is found with the corresponding id, the controller will return NotFound (404)
        /// </returns>
        [HttpGet, AdminFilter, Route("{id}")]
        public IHttpActionResult Get(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var manager = GetManager();
            if (manager == null) return BadRequest("Provided token is invalid!");

            var employeeTitle = _employeeTitleRepository.Read(id, manager.Institution.Id);
            if (employeeTitle != null)
            {
                return Ok(employeeTitle);
            }
            else
            {
                return NotFound();
            }

        }


        // PUT api/employeetitles/5
        /// <summary>
        /// Updates the employee title with the specified id.
        /// </summary>
        /// <param name="id">The id of the employee title.</param>
        /// <returns>
        /// Returns 'No Content' (204) if the employee title gets updated.
        /// If no employee title is found with the given id, the controller will return NotFound (404)
        /// </returns>
        [HttpPut, AdminFilter, Route("{id}")]
        public IHttpActionResult Put(int id, UpdateEmployeeTitleDTO employeeTitleDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var manager = GetManager();
            if (manager == null) return BadRequest("Provided token is invalid!");

            var employeeTitle = _employeeTitleRepository.Read(id, manager.Institution.Id);
            if (employeeTitle != null)
            {
                employeeTitle.Title = employeeTitleDto.Title;
                if (_employeeTitleRepository.Update(employeeTitle) > 0)
                {
                    return ResponseMessage(new HttpResponseMessage(HttpStatusCode.NoContent));
                }
                else
                {
                    return BadRequest("Could not update employee title");
                }
            }
            return NotFound();
        }


        // DELETE /api/employeetitles/{id}
        /// <summary>
        /// Deletes the employee title with the specified id.
        /// </summary>
        /// <param name="id">The id of the employee title.</param>
        /// <returns>Returns 'No Content' (204) if the employee title gets deleted.</returns>
        [HttpDelete, AdminFilter, Route("{id}")]
        public IHttpActionResult Delete(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var manager = GetManager();
            if (manager == null) return BadRequest("Provided token is invalid!");

            _employeeTitleRepository.Delete(id, manager.Institution.Id);
            return ResponseMessage(new HttpResponseMessage(HttpStatusCode.NoContent));
        }

        private Manager GetManager()
        {
            var token = Request.Headers.Authorization.ToString();
            return _managerRepository.Read(token);
        }

    }
}