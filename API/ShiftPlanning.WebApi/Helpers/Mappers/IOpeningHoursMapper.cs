using System.Collections.Generic;
using ShiftPlanning.DTOs.Public.OpeningHours;
using ShiftPlanning.Model.Models;

namespace ShiftPlanning.WebApi.Helpers.Mappers
{
    /// <summary>
    /// Mapper interface for opening hour classes and DTOs.
    /// </summary>
    public interface IOpeningHoursMapper
    {
        /// <summary>
        /// Maps a collection of Shifts into a collection of OpeningHoursDTOs.
        /// </summary>
        /// <param name="source">The input collection.</param>
        /// <returns>The mapped collection.</returns>
        IEnumerable<OpeningHoursDTO> MapToDto(IEnumerable<Shift> source);

        IntervalOpeningHoursDTO MapToIntervalDto(ICollection<Shift> shifts, int interval);
    }
}