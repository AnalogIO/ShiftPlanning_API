using System;
using System.Collections.Generic;

namespace DataTransferObjects.OpeningHours
{
    /// <summary>
    /// Contains the public information about a single shift, and the employees.
    /// </summary>
    public class OpeningHoursDTO
    {
        /// <summary>
        /// The ID of the shift.
        /// </summary>
        // TODO: Is this ever needed?
        public int Id { get; set; }

        /// <summary>
        /// The starting date and time of the shift.
        /// </summary>
        public DateTime Start { get; set; }

        /// <summary>
        /// The ending date and time.
        /// </summary>
        public DateTime End { get; set; }

        /// <summary>
        /// The attending employees.
        /// </summary>
        public IEnumerable<OpeningHourEmployeeDTO> Employees { get; set; }

        /// <summary>
        /// The employees that have checked in for this shift.
        /// </summary>
        public IEnumerable<OpeningHourEmployeeDTO> CheckedInEmployees { get; set; }
    }
}
