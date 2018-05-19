using System;
using System.Collections.Generic;
using Data.Models;
using DataTransferObjects.Shift;

namespace Data.Services
{
    /// <summary>
    /// Interaction logic for shifts.
    /// </summary>
    public interface IShiftService
    {
        /// <summary>
        /// Returns all shifts from an organization.
        /// </summary>
        /// <param name="shortKey">The short key of the organization</param>
        /// <returns>A collection of shifts. null if the organization was not found.</returns>
        IEnumerable<Shift> GetByOrganization(string shortKey);

        /// <summary>
        /// Returns all shifts from an organization.
        /// </summary>
        /// <param name="id">The ID of the organization</param>
        /// <returns>A collection of shifts. null if the orgnanization was not found.</returns>
        IEnumerable<Shift> GetByOrganization(int id);

        /// <summary>
        /// Returns all shifts that crosses or are inside the date denoted by <paramref name="date"/>
        /// </summary>
        /// <param name="shortKey">The short key of the organization</param>
        /// <param name="date">The date of interesting shifts.</param>
        /// <returns>A collection of shifts. null if the organization was not found.</returns>
        IEnumerable<Shift> GetByOrganization(string shortKey, DateTime date);

        /// <summary>
        /// Returns all shifts that crosses or are inside the date denoted by <paramref name="date"/>
        /// </summary>
        /// <param name="id">The ID of the organization</param>
        /// <param name="date">The date of interesting shifts.</param>
        /// <returns>A collection of shifts. null if the organization was not found.</returns>
        IEnumerable<Shift> GetByOrganization(int id, DateTime date);

        /// <summary>
        /// Returns all shifts that crosses or are inside the range denoted by <paramref name="from"/> and <paramref name="to"/>
        /// </summary>
        /// <param name="shortKey">The short key of the organization</param>
        /// <param name="from">Beginning of range.</param>
        /// <param name="to">End of range.</param>
        /// <returns>A collection of shifts. null if the organization was not found.</returns>
        IEnumerable<Shift> GetByOrganization(string shortKey, DateTime from, DateTime to);

        /// <summary>
        /// Returns all shifts that crosses or are inside the range denoted by <paramref name="from"/> and <paramref name="to"/>
        /// </summary>
        /// <param name="id">The ID of the organization</param>
        /// <param name="from">Beginning of range.</param>
        /// <param name="to">End of range.</param>
        /// <returns>A collection of shifts. null if the organization was not found.</returns>
        IEnumerable<Shift> GetByOrganization(int id, DateTime from, DateTime to);

        /// <summary>
        /// Retrieve ongoing shifts for the given organization.
        /// </summary>
        /// <param name="shortKey">The short key of the organization.</param>
        /// <returns>
        /// A collection of shifts, mostly (hopefully) containing only a single element.
        /// null if the organization was not found.
        /// </returns>
        IEnumerable<Shift> GetOngoingShiftsByOrganization(string shortKey);

        /// <summary>
        /// Retrieve ongoing shifts for the given organization.
        /// </summary>
        /// <param name="id">The ID of the organization.</param>
        /// <returns>
        /// A collection of shifts, mostly (hopefully) containing only a single element.
        /// null if the organization was not found.
        /// </returns>
        IEnumerable<Shift> GetOngoingShiftsByOrganization(int id);

        Shift GetShift(int shiftId, int organizationId);

        void DeleteShift(int shiftId, int organizationId);
        Shift UpdateShift(int shiftId, int organizationId, UpdateShiftDTO updateShiftDto);
        Shift CreateShift(Organization organization, CreateShiftDTO shiftDto);
        Shift CreateLimitedShift(Organization organization, CreateShiftDTO shiftDto, int maxLengthMinutes);
        CheckIn CheckInEmployee(int shiftId, int employeeId, int organizationId);
        void CheckOutEmployee(int shiftId, int employeeId, int organizationId);
        Shift AddEmployeesToShift(int shiftId, int organizationId, AddEmployeesDTO employees);
        Shift CreateShiftOutsideSchedule(CreateShiftOutsideScheduleDTO shiftDto, Organization organization);
        bool IsOrganisationOpen(string shortKey);
    }
}