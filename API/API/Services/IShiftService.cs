using System;
using System.Collections.Generic;
using Data.Models;

namespace API.Services
{
    /// <summary>
    /// Interaction logic for shifts.
    /// </summary>
    public interface IShiftService
    {
        /// <summary>
        /// Returns all shifts from an institution.
        /// </summary>
        /// <param name="shortKey">The short key of the institution</param>
        /// <returns>A collection of shifts. null if the institution was not found.</returns>
        IEnumerable<Shift> GetByInstitution(string shortKey);

        /// <summary>
        /// Returns all shifts from an institution.
        /// </summary>
        /// <param name="id">The ID of the institution</param>
        /// <returns>A collection of shifts. null if the institution was not found.</returns>
        IEnumerable<Shift> GetByInstitution(int id);

        /// <summary>
        /// Returns all shifts that crosses or are inside the date denoted by <paramref name="date"/>
        /// </summary>
        /// <param name="shortKey">The short key of the institution</param>
        /// <param name="date">The date of interesting shifts.</param>
        /// <returns>A collection of shifts. null if the institution was not found.</returns>
        IEnumerable<Shift> GetByInstitution(string shortKey, DateTime date);

        /// <summary>
        /// Returns all shifts that crosses or are inside the date denoted by <paramref name="date"/>
        /// </summary>
        /// <param name="id">The ID of the institution</param>
        /// <param name="date">The date of interesting shifts.</param>
        /// <returns>A collection of shifts. null if the institution was not found.</returns>
        IEnumerable<Shift> GetByInstitution(int id, DateTime date);

        /// <summary>
        /// Returns all shifts that crosses or are inside the range denoted by <paramref name="from"/> and <paramref name="to"/>
        /// </summary>
        /// <param name="shortKey">The short key of the institution</param>
        /// <param name="from">Beginning of range.</param>
        /// <param name="to">End of range.</param>
        /// <returns>A collection of shifts. null if the institution was not found.</returns>
        IEnumerable<Shift> GetByInstitution(string shortKey, DateTime from, DateTime to);

        /// <summary>
        /// Returns all shifts that crosses or are inside the range denoted by <paramref name="from"/> and <paramref name="to"/>
        /// </summary>
        /// <param name="id">The ID of the institution</param>
        /// <param name="from">Beginning of range.</param>
        /// <param name="to">End of range.</param>
        /// <returns>A collection of shifts. null if the institution was not found.</returns>
        IEnumerable<Shift> GetByInstitution(int id, DateTime from, DateTime to);

        /// <summary>
        /// Retrieve ongoing shifts for the given institution.
        /// </summary>
        /// <param name="shortKey">The short key of the institution.</param>
        /// <returns>
        /// A collection of shifts, mostly (hopefully) containing only a single element.
        /// null if the institution was not found.
        /// </returns>
        IEnumerable<Shift> GetOngoingShiftsByInstitution(string shortKey);

        /// <summary>
        /// Retrieve ongoing shifts for the given institution.
        /// </summary>
        /// <param name="id">The ID of the institution.</param>
        /// <returns>
        /// A collection of shifts, mostly (hopefully) containing only a single element.
        /// null if the institution was not found.
        /// </returns>
        IEnumerable<Shift> GetOngoingShiftsByInstitution(int id);
    }
}