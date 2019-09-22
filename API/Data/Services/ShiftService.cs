using System;
using System.Collections.Generic;
using System.Linq;
using Data.Models;
using Data.Repositories;
using DataTransferObjects.Shift;
using System.Data;
using Data.Exceptions;
using Microsoft.Practices.ObjectBuilder2;
using System.Web;
using System.Data.Entity.Core;

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
        private readonly ICheckInRepository _checkInRepository;

        /// <summary>
        /// Injection constructor.
        /// </summary>
        /// <param name="shiftRepository">An IShiftRepository implementation.</param>
        /// <param name="institutionRepository">An IInstitution implementation.</param>
        public ShiftService(IShiftRepository shiftRepository, IOrganizationRepository organizationRepository, IEmployeeRepository employeeRepository, ICheckInRepository checkInRepository)
        {
            _shiftRepository = shiftRepository;
            _organizationRepository = organizationRepository;
            _employeeRepository = employeeRepository;
            _checkInRepository = checkInRepository;
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
            return GetByOrganization(shortKey).AsQueryable().Where(shift => shift.End >= from && shift.Start <= to);
        }

        /// <inheritdoc cref="IShiftService.GetByOrganization(int, DateTime, DateTime)"/>
        public IEnumerable<Shift> GetByOrganization(int id, DateTime from, DateTime to)
        {
            return GetByOrganization(id).AsQueryable().Where(shift => shift.End >= from && shift.Start <= to);
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
            return GetByOrganization(shortKey).AsQueryable().Where(shift => shift.Start <= now && now <= shift.End);
        }

        /// <inheritdoc cref="IShiftService.GetOngoingShiftsByOrganization(int)"/>
        public IEnumerable<Shift> GetOngoingShiftsByOrganization(int id)
        {
            var now = DateTime.Now;
            var nextHour = now.AddHours(1);
            return GetByOrganization(id).AsQueryable().Where(shift => (shift.Start <= now || shift.Start <= nextHour) && now <= shift.End);
        }

        public IEnumerable<Shift> GetIntersectingShifts(int organizationId, DateTime start, DateTime end)
        {
            var existingShifts =
                _shiftRepository.ReadFromOrganization(organizationId).AsQueryable()
                    .Where(s => start == s.Start || end == s.End
                    || (s.Start < start && ((s.End > start && s.End < end) || (s.End > end)))
                    || (s.Start > start && ((end > s.Start && end < s.End) || end > s.End)));
            return existingShifts;
        }

        public CheckIn CheckInEmployee(int shiftId, int employeeId, int organizationId)
        {
            var shift = _shiftRepository.Read(shiftId, organizationId);
            if (shift == null) throw new ObjectNotFoundException("Could not find a shift corresponding to the given id");
            if (shift.CheckIns.FirstOrDefault(x => x.Employee.Id == employeeId) != null) throw new ForbiddenException("Could not check in because the given employee is already checked in");
            var employee = _employeeRepository.Read(employeeId, organizationId);
            if (employee == null) throw new ObjectNotFoundException("Could not find an employee corresponding to the given id");
            var now = DateTime.Now;
            if(now > shift.End) throw new ForbiddenException("You cannot check into a shift that has ended");
            var checkIn = new CheckIn { Employee = employee, Time = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second) };
            shift.CheckIns.Add(checkIn);
            return _shiftRepository.Update(shift) > 0 ? shift.CheckIns.LastOrDefault() : null;
        }

        public void CheckOutEmployee(int shiftId, int employeeId, int organizationId)
        {
            var shift = _shiftRepository.Read(shiftId, organizationId);
            if (shift == null) throw new ObjectNotFoundException("Could not find a shift corresponding to the given id");
            var checkin = shift.CheckIns.FirstOrDefault(x => x.Employee.Id == employeeId);
            if (checkin == null) throw new ForbiddenException("Could not check out because the given employee is not checked in");
            var employee = _employeeRepository.Read(employeeId, organizationId);
            if (employee == null) throw new ObjectNotFoundException("Could not find an employee corresponding to the given id");
            var now = DateTime.Now;
            if (now > shift.End) throw new ForbiddenException("You cannot check out from a shift that has ended");
            _checkInRepository.Delete(checkin);
        }

        public Shift AddEmployeesToShift(int shiftId, int organizationId, AddEmployeesDTO employeesDto)
        {
            var shift = _shiftRepository.Read(shiftId, organizationId);
            if (shift == null) throw new ObjectNotFoundException("Could not find a shift corresponding to the given id");
            if (shift.Employees.Any(x => employeesDto.EmployeeIds.Contains(x.Id))) throw new ForbiddenException("You cannot add employees to a shift if they are already added to the shift");
            var employees =
                _employeeRepository.ReadFromOrganization(organizationId)
                    .Where(e => employeesDto.EmployeeIds.Contains(e.Id));
            if (!employees.Any()) throw new ObjectNotFoundException("Could not find any employees corresponding to the given ids");
            foreach (var employee in employees)
            {
                shift.Employees.Add(employee);
            }
            return _shiftRepository.Update(shift) > 0 ? shift : null;
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

        public bool IsOrganisationOpen(string shortKey)
        {
            return _shiftRepository.IsOrganisationOpen(shortKey);
        }

        public Shift GetShift(int shiftId, int organizationId)
        {
            return _shiftRepository.Read(shiftId, organizationId);
        }

        public void DeleteShift(int shiftId, int organizationId)
        {
            if(((string)HttpContext.Current.Items["Audit"]).Equals("Application"))
            {
                if(_shiftRepository.Read(shiftId, organizationId).Schedule != null) throw new ForbiddenException("Only user-created shifts can be deleted!");
            }
            _shiftRepository.Delete(shiftId, organizationId);
        }

        public Shift UpdateShift(int shiftId, int organizationId, UpdateShiftDTO updateShiftDto)
        {
            var now = DateTime.Now;

            var shift = _shiftRepository.Read(shiftId, organizationId);
            if (shift == null) throw new ObjectNotFoundException("Could not find a shift corresponding to the given id");

            if(String.IsNullOrEmpty(updateShiftDto.Start)) throw new ForbiddenException("A start time is required to update the shift");
            if(String.IsNullOrEmpty(updateShiftDto.End)) throw new ForbiddenException("An end time is required to update the shift");

            var start = DateTimeOffset.Parse(updateShiftDto.Start).LocalDateTime;
            var end = DateTimeOffset.Parse(updateShiftDto.End).LocalDateTime;

            if (start > end) throw new ForbiddenException("The shift cannot end before it has started");
            if (end < now) throw new ForbiddenException("The end of the shift should be in the future");

            var intersectingShifts = GetIntersectingShifts(organizationId, start, end);

            if (intersectingShifts.Any(s => s.Id != shift.Id)) throw new ForbiddenException("You cannot update a shift that will intersect other shifts");

            var employees = _employeeRepository.ReadFromOrganization(organizationId).Where(x => updateShiftDto.EmployeeIds.Contains(x.Id)).ToList();
            var employeesToRemove = shift.Employees.Where(e => !updateShiftDto.EmployeeIds.Contains(e.Id)).ToList();

            foreach(var employee in employeesToRemove)
            {
                var checkin = shift.CheckIns.FirstOrDefault(x => x.Employee.Id == employee.Id);
                if (checkin != null) _checkInRepository.Delete(checkin); // if employee is checked in, let's remove that before removing them from the shift
                employee.Shifts.Remove(shift);
            }

            shift.Start = start;
            shift.End = end;
            shift.Employees = employees;
            
            _shiftRepository.Update(shift);

            return shift;
        }

        public Shift PatchShift(int shiftId, int organizationId, PatchShiftDTO patchShiftDto)
        {
            var now = DateTime.Now;

            var shift = _shiftRepository.Read(shiftId, organizationId);
            if (shift == null) throw new ObjectNotFoundException("Could not find a shift corresponding to the given id");

            if (!String.IsNullOrEmpty(patchShiftDto.End))
            {
                var end = DateTimeOffset.Parse(patchShiftDto.End).LocalDateTime;
                if (end < now) throw new ForbiddenException("The end of the shift should be in the future");

                var intersectingShifts = GetIntersectingShifts(organizationId, shift.Start, end);
                if (intersectingShifts.Any(s => s.Id != shift.Id)) throw new ForbiddenException("You cannot update a shift that will intersect other shifts");

                shift.End = end;
            }

            if (!String.IsNullOrEmpty(patchShiftDto.Start))
            {
                var start = DateTimeOffset.Parse(patchShiftDto.Start).LocalDateTime;
                if (start > shift.End) throw new ForbiddenException("The shift cannot end before it has started");

                var intersectingShifts = GetIntersectingShifts(organizationId, start, shift.End);
                if (intersectingShifts.Any(s => s.Id != shift.Id)) throw new ForbiddenException("You cannot update a shift that will intersect other shifts");

                shift.Start = start;
            }

            if (patchShiftDto.EmployeeIds != null)
            {
                var employees = _employeeRepository.ReadFromOrganization(organizationId).Where(x => patchShiftDto.EmployeeIds.Contains(x.Id)).ToList();
                var employeesToRemove = shift.Employees.Where(e => !patchShiftDto.EmployeeIds.Contains(e.Id)).ToList();

                foreach (var employee in employeesToRemove)
                {
                    var checkin = shift.CheckIns.FirstOrDefault(x => x.Employee.Id == employee.Id);
                    if (checkin != null) _checkInRepository.Delete(checkin); // if employee is checked in, let's remove that before removing them from the shift
                    employee.Shifts.Remove(shift);
                }

                shift.Employees = employees;
            }

            _shiftRepository.Update(shift);

            return shift;
        }

        public Shift CreateShift(Organization organization, CreateShiftDTO shiftDto)
        {
            var now = DateTime.Now;

            var start = DateTimeOffset.Parse(shiftDto.Start).LocalDateTime;
            var end = DateTimeOffset.Parse(shiftDto.End).LocalDateTime;

            if (start > end) throw new ForbiddenException("The shift cannot end before it has started");
            if (end < now) throw new ForbiddenException("The end of the shift should be in the future");

            var intersectingShifts = GetIntersectingShifts(organization.Id, start, end);

            if (intersectingShifts.Any()) throw new ForbiddenException("You cannot create a shift the intersects with another");

            var employees = _employeeRepository.ReadFromOrganization(organization.Id).Where(x => shiftDto.EmployeeIds.Contains(x.Id)).ToList();

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
            var startSpan = TimeSpan.Parse(shiftDto.Start);
            var endSpan = TimeSpan.Parse(shiftDto.End);

            var now = DateTime.Now;

            var start = new DateTime(now.Year, now.Month, now.Day, startSpan.Hours, startSpan.Minutes, startSpan.Seconds);
            var end = new DateTime(now.Year, now.Month, now.Day, endSpan.Hours, endSpan.Minutes, endSpan.Seconds);

            var intersectingShifts = GetIntersectingShifts(organization.Id, start, end);

            if (intersectingShifts.Any()) throw new ForbiddenException("You cannot create a shift the intersects with another");
            
            if (start > end) throw new ForbiddenException("The shift cannot end before it has started");
            if (end < now) throw new ForbiddenException("The end of the shift should be in the future");
            if((end - start).TotalMinutes > maxLengthMinutes) throw new ForbiddenException($"You cannot create a shift that has a duration over {maxLengthMinutes} minutes");
            if((end - start).TotalMinutes < 30) throw new ForbiddenException("The shift has to last minimum 30 minutes");

            var employees = _employeeRepository.ReadFromOrganization(organization.Id).Where(x => shiftDto.EmployeeIds.Contains(x.Id)).ToList();

            if (!employees.Any()) throw new ForbiddenException("You have to select at least 1 employee to create the shift");

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