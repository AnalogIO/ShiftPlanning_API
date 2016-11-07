using System.Collections.Generic;
using Data.Models;
using DataTransferObjects.OpeningHours;

namespace API.Mapping
{
    public interface IOpeningHoursMapper
    {
        IEnumerable<OpeningHoursDTO> MapToDto(IEnumerable<Shift> source);
    }
}