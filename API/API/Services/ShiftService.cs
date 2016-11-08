using System;
using System.Collections.Generic;
using System.Linq;
using Data.Models;
using Data.Repositories;

namespace API.Services
{
    /// <summary>
    /// Implementation of IShiftService that uses Repositories for data access.
    /// </summary>
    public class ShiftService : IShiftService
    {
        private readonly IShiftRepository _shiftRepository;
        private readonly IInstitutionRepository _institutionRepository;
        private readonly IEmployeeRepository _employeeRepository;

        /// <summary>
        /// Injection constructor.
        /// </summary>
        /// <param name="shiftRepository">An IShiftRepository implementation.</param>
        /// <param name="institutionRepository">An IInstitution implementation.</param>
        public ShiftService(IShiftRepository shiftRepository, IInstitutionRepository institutionRepository, IEmployeeRepository employeeRepository)
        {
            _shiftRepository = shiftRepository;
            _institutionRepository = institutionRepository;
            _employeeRepository = employeeRepository;
        }

        /// <inheritdoc cref="IShiftService.GetByInstitution(string)"/>
        public IEnumerable<Shift> GetByInstitution(string shortKey)
        {
            if (_institutionRepository.Exists(shortKey))
                return _shiftRepository.ReadFromInstitution(shortKey);
            return null;
        }

        /// <inheritdoc cref="IShiftService.GetByInstitution(int)"/>
        public IEnumerable<Shift> GetByInstitution(int id)
        {
            if (_institutionRepository.Exists(id))
                return _shiftRepository.ReadFromInstitution(id);
            return null;
        }

        /// <inheritdoc cref="IShiftService.GetByInstitution(string, DateTime, DateTime)"/>
        public IEnumerable<Shift> GetByInstitution(string shortKey, DateTime from, DateTime to)
        {
            return GetByInstitution(shortKey).Where(shift => shift.End >= from && shift.Start <= to);
        }

        /// <inheritdoc cref="IShiftService.GetByInstitution(int, DateTime, DateTime)"/>
        public IEnumerable<Shift> GetByInstitution(int id, DateTime from, DateTime to)
        {
            return GetByInstitution(id).Where(shift => shift.End >= from && shift.Start <= to);
        }

        /// <inheritdoc cref="IShiftService.GetByInstitution(string, DateTime)"/>
        public IEnumerable<Shift> GetByInstitution(string shortKey, DateTime date)
        {
            return GetByInstitution(shortKey, date.Date, date.Date.AddDays(1));
        }

        /// <inheritdoc cref="IShiftService.GetByInstitution(int, DateTime)"/>
        public IEnumerable<Shift> GetByInstitution(int id, DateTime date)
        {
            return GetByInstitution(id, date.Date, date.Date.AddDays(1));
        }

        /// <inheritdoc cref="IShiftService.GetOngoingShiftsByInstitution(string)"/>
        public IEnumerable<Shift> GetOngoingShiftsByInstitution(string shortKey)
        {
            var now = DateTime.Now;
            return GetByInstitution(shortKey).Where(shift => shift.Start <= now && now <= shift.End);
        }

        /// <inheritdoc cref="IShiftService.GetOngoingShiftsByInstitution(int)"/>
        public IEnumerable<Shift> GetOngoingShiftsByInstitution(int id)
        {
            var now = DateTime.Now;
            return GetByInstitution(id).Where(shift => shift.Start <= now && now <= shift.End);
        }

        public CheckIn CheckInEmployee(int shiftId, int employeeId, int institutionId)
        {
            var shift = _shiftRepository.Read(shiftId, institutionId);
            if (shift == null) return null;
            if (shift.CheckIns.Where(x => x.Employee.Id == employeeId).FirstOrDefault() != null) return null;
            var employee = _employeeRepository.Read(employeeId, institutionId);
            if (employee == null) return null;
            var now = DateTime.UtcNow;
            var checkIn = new CheckIn { Employee = employee, Time = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second) };
            shift.CheckIns.Add(checkIn);
            return _shiftRepository.Update(shift) > 0 ? shift.CheckIns.LastOrDefault() : null;
        }
    }
}