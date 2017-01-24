using System;
using System.Collections.Generic;
using System.Linq;
using Data.Models;
using Data.Repositories;
using DataTransferObjects.Shift;
using System.Data;
using System.Runtime.CompilerServices;
using Data.Exceptions;

namespace Data.Services
{
    /// <summary>
    /// Implementation of IShiftService that uses Repositories for data access.
    /// </summary>
    public class ShiftService : IShiftService
    {
        private readonly IShiftRepository _shiftRepository;
        private readonly IOrganizationRepository _organizationRepository;
        private readonly IEmployeeRepository _employeeRepository;

        /// <summary>
        /// Injection constructor.
        /// </summary>
        /// <param name="shiftRepository">An IShiftRepository implementation.</param>
        /// <param name="institutionRepository">An IInstitution implementation.</param>
        public ShiftService(IShiftRepository shiftRepository, IOrganizationRepository organizationRepository, IEmployeeRepository employeeRepository)
        {
            _shiftRepository = shiftRepository;
            _organizationRepository = organizationRepository;
            _employeeRepository = employeeRepository;
        }

        /// <inheritdoc cref="IShiftService.GetByOrganization(string)"/>
        public IEnumerable<Shift> GetByOrganization(string shortKey)
        {
            if (_organizationRepository.Exists(shortKey))
                return _shiftRepository.ReadFromOrganization(shortKey);
            throw new ObjectNotFoundException("Could not find an organization corresponding to the given shortkey");
        }

        /// <inheritdoc cref="IShiftService.GetByOrganization(int)"/>
        public IEnumerable<Shift> GetByOrganization(int id)
        {
            if (_organizationRepository.Exists(id))
                return _shiftRepository.ReadFromOrganization(id);
            throw new ObjectNotFoundException("Could not find an organization corresponding to the given id");
        }

        /// <inheritdoc cref="IShiftService.GetByOrganization(string, DateTime, DateTime)"/>
        public IEnumerable<Shift> GetByOrganization(string shortKey, DateTime from, DateTime to)
        {
            return GetByOrganization(shortKey).Where(shift => shift.End >= from && shift.Start <= to);
        }

        /// <inheritdoc cref="IShiftService.GetByOrganization(int, DateTime, DateTime)"/>
        public IEnumerable<Shift> GetByOrganization(int id, DateTime from, DateTime to)
        {
            return GetByOrganization(id).Where(shift => shift.End >= from && shift.Start <= to);
        }

        /// <inheritdoc cref="IShiftService.GetByOrganization(string, DateTime)"/>
        public IEnumerable<Shift> GetByOrganization(string shortKey, DateTime date)
        {
            return GetByOrganization(shortKey, date.Date, date.Date.AddDays(1));
        }

        /// <inheritdoc cref="IShiftService.GetByOrganization(int, DateTime)"/>
        public IEnumerable<Shift> GetByOrganization(int id, DateTime date)
        {
            return GetByOrganization(id, date.Date, date.Date.AddDays(1));
        }

        /// <inheritdoc cref="IShiftService.GetOngoingShiftsByOrganization(string)"/>
        public IEnumerable<Shift> GetOngoingShiftsByOrganization(string shortKey)
        {
            var now = DateTime.Now;
            return GetByOrganization(shortKey).Where(shift => shift.Start <= now && now <= shift.End);
        }

        /// <inheritdoc cref="IShiftService.GetOngoingShiftsByOrganization(int)"/>
        public IEnumerable<Shift> GetOngoingShiftsByOrganization(int id)
        {
            var now = DateTime.Now;
            var nextHour = now.AddHours(1);
            return GetByOrganization(id).Where(shift => (shift.Start <= now || shift.Start <= nextHour) && now <= shift.End);
        }

        public CheckIn CheckInEmployee(int shiftId, int employeeId, int institutionId)
        {
            var shift = _shiftRepository.Read(shiftId, institutionId);
            if (shift == null) throw new ObjectNotFoundException("Could not find a shift corresponding to the given id");
            if (shift.CheckIns.FirstOrDefault(x => x.Employee.Id == employeeId) != null) throw new ForbiddenException("Could not check in because the given employee is already checked in");
            var employee = _employeeRepository.Read(employeeId, institutionId);
            if (employee == null) throw new ObjectNotFoundException("Could not find an employee corresponding to the given id");
            var now = DateTime.Now;
            var checkIn = new CheckIn { Employee = employee, Time = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second) };
            shift.CheckIns.Add(checkIn);
            return _shiftRepository.Update(shift) > 0 ? shift.CheckIns.LastOrDefault() : null;
        }

        public Shift CreateShiftOutsideSchedule(CreateShiftOutsideScheduleDTO shiftDto, Organization organization)
        {
            var employees = _employeeRepository.ReadFromOrganization(organization.Id).Where(x => shiftDto.EmployeeIds.Contains(x.Id)).ToList();
            if (employees == null) throw new ObjectNotFoundException("Could not find the employees corresponding to the given ids");
            var now = DateTime.Now;
            var start = Toolbox.RoundUp(now);
            var end = start.AddMinutes(shiftDto.OpenMinutes);

            var shift = new Shift { Start = start, End = end, CheckIns = new List<CheckIn>(), Employees = employees, Organization = organization };
            return _shiftRepository.Create(shift);
        }

        public Shift GetShift(int shiftId, int organizationId)
        {
            return _shiftRepository.Read(shiftId, organizationId);
        }

        public void DeleteShift(int shiftId, int organizationId)
        {
            _shiftRepository.Delete(shiftId, organizationId);
        }

        public Shift UpdateShift(int shiftId, int organizationId, UpdateShiftDTO updateShiftDto)
        {
            var shift = _shiftRepository.Read(shiftId, organizationId);
            if (shift == null) throw new ObjectNotFoundException("Could not find a shift corresponding to the given id");

            var employees = _employeeRepository.ReadFromOrganization(organizationId).Where(x => updateShiftDto.EmployeeIds.Contains(x.Id)).ToList();

            var start = DateTimeOffset.Parse(updateShiftDto.Start).UtcDateTime;
            var end = DateTimeOffset.Parse(updateShiftDto.End).UtcDateTime;

            shift.Employees = employees;
            shift.CheckIns = shift.CheckIns.Where(x => updateShiftDto.CheckInIds.Contains(x.Id)).ToList();
            shift.Start = start;
            shift.End = end;

            return _shiftRepository.Update(shift) > 0 ? shift : null;
        }

        public Shift CreateShift(Organization organization, CreateShiftDTO shiftDto)
        {
            var employees = _employeeRepository.ReadFromOrganization(organization.Id).Where(x => shiftDto.EmployeeIds.Contains(x.Id)).ToList();

            var start = DateTimeOffset.Parse(shiftDto.Start).LocalDateTime;
            var end = DateTimeOffset.Parse(shiftDto.End).LocalDateTime;

            var shift = new Shift
            {
                CheckIns = new List<CheckIn>(),
                Start = start,
                End = end,
                Employees = employees,
                Organization = organization,
                Schedule = null
            };

            return _shiftRepository.Create(shift);
        }

        public Shift CreateLimitedShift(Organization organization, CreateShiftDTO shiftDto, int maxLengthMinutes)
        {
            var employees = _employeeRepository.ReadFromOrganization(organization.Id).Where(x => shiftDto.EmployeeIds.Contains(x.Id)).ToList();

            var start = DateTimeOffset.Parse(shiftDto.Start).LocalDateTime;
            var end = DateTimeOffset.Parse(shiftDto.End).LocalDateTime;

            if((end - start).TotalMinutes > maxLengthMinutes) throw new ForbiddenException($"You cannot create a shift that has a duration over {maxLengthMinutes} minutes");

            var now = DateTime.Now;

            var checkIns = employees.Select(e => new CheckIn
            {
                Employee = e,
                Time = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second)
            }).ToList();

            var shift = new Shift
            {
                CheckIns = checkIns,
                Start = start,
                End = end,
                Employees = employees,
                Organization = organization,
                Schedule = null
            };

            return _shiftRepository.Create(shift);
        }
    }
}