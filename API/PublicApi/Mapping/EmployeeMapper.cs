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

            return employees.Select(emp => new EmployeeDTO
            {
                Name = emp.FirstName,
                Title = emp.EmployeeTitle.Title,
                PhotoRef = $"{url.Scheme}://{url.Host}{portString}/{PhotosController.RoutePrefix}/{emp.Photo.Organization.ShortKey}/{emp.Photo.Id}"
            });
        }

        public EmployeeDTO Map(Employee employee)
        {
            var url = HttpContext.Current.Request.Url;

            var portString = url.IsDefaultPort ? "" : $":{url.Port}";

            return new EmployeeDTO
            {
                Name = employee.FirstName,
                Title = employee.EmployeeTitle.Title,
                PhotoRef = $"{url.Scheme}://{url.Host}{portString}/{PhotosController.RoutePrefix}/{employee.Photo.Organization.ShortKey}/{employee.Photo.Id}"
            };
        }
    }
}