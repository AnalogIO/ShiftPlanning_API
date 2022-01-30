using System.Collections.Generic;
using ShiftPlanning.DTOs.Employee;
using ShiftPlanning.Model.Models;

namespace ShiftPlanning.WebApi.Helpers.Mappers
{
    public interface IVolunteerMapper
    {
        EmployeeDTO Map(Employee employee);
        IEnumerable<EmployeeDTO> Map(IEnumerable<Employee> employees);
    }
}
