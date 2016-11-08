using Data.Models;
using DataTransferObjects.Volunteers;
using System.Collections.Generic;
using System;

namespace API.Mapping
{
    public interface IVolunteerMapper
    {
        VolunteerDTO Map(Employee employee);
        IEnumerable<VolunteerDTO> Map(IEnumerable<Employee> employees);
    }
}
