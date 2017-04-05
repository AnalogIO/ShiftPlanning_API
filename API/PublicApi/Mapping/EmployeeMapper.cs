using System.Collections.Generic;
using System.Linq;
using System.Web;
using Data.Models;
using DataTransferObjects.Public.Employees;
using PublicApi.Controllers;

namespace PublicApi.Mapping
{
    public class EmployeeMapper : IVolunteerMapper
    {
        public IEnumerable<EmployeeDTO> Map(IEnumerable<Employee> employees)
        {
            var url = HttpContext.Current.Request.Url;

            var portString = url.IsDefaultPort ? "" : $":{url.Port}";

            var path = url.LocalPath;
            var deployedPath = path.Substring(1, path.IndexOf("api") - 2);

            return employees.Select(emp => new EmployeeDTO
            {
                FirstName = emp.FirstName,
                LastName = emp.LastName,
                Title = emp.EmployeeTitle.Title,
                PhotoRef = $"{url.Scheme}://{url.Host}{portString}/{deployedPath}/{PhotosController.RoutePrefix}/{emp.Photo.Organization.ShortKey}/{emp.Photo.Id}"
            });
        }

        public EmployeeDTO Map(Employee employee)
        {
            var url = HttpContext.Current.Request.Url;

            var portString = url.IsDefaultPort ? "" : $":{url.Port}";

            var path = url.LocalPath;
            var deployedPath = path.Substring(1, path.IndexOf("api") - 2);

            return new EmployeeDTO
            {
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Title = employee.EmployeeTitle.Title,
                PhotoRef = $"{url.Scheme}://{url.Host}{portString}/{deployedPath}/{PhotosController.RoutePrefix}/{employee.Photo.Organization.ShortKey}/{employee.Photo.Id}"
            };
        }
    }
}