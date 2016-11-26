using System.Collections.Generic;
using Data.Models;
using DataTransferObjects.Public.Employees;

namespace PublicApi.Mapping
{
    public interface IVolunteerMapper
    {
        EmployeeDTO Map(Employee employee);
        IEnumerable<EmployeeDTO> Map(IEnumerable<Employee> employees);
    }
}
