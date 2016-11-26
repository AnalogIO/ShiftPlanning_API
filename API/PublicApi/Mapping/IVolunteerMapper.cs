using System.Collections.Generic;
using Data.Models;
using DataTransferObjects.Volunteers;

namespace PublicApi.Mapping
{
    public interface IVolunteerMapper
    {
        VolunteerDTO Map(Employee employee);
        IEnumerable<VolunteerDTO> Map(IEnumerable<Employee> employees);
    }
}
