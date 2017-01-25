using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Data.Models;
using Data.Repositories;
using System.Data.Entity;
using Data.Exceptions;

namespace Data.Npgsql.Repositories
{
    public class ShiftRepository : IShiftRepository, IDisposable
    {
        private readonly IShiftPlannerDataContext _context;

        public ShiftRepository(IShiftPlannerDataContext context)
        {
            _context = context;
        }

        public Shift Create(Shift shift)
        {
            _context.Shifts.Add(shift);
            return _context.SaveChanges() > 0 ? shift : null;
        }

        public IEnumerable<Shift> Create(IEnumerable<Shift> shifts)
        {
            _context.Shifts.AddRange(shifts);
            return _context.SaveChanges() > 0 ? shifts : null;
        }

        public void Delete(IEnumerable<Shift> shifts)
        {
            _context.Shifts.RemoveRange(shifts);
            _context.SaveChanges();
        }

        public void Delete(int id, int organizationId)
        {
            var shift = _context.Shifts.FirstOrDefault(x => x.Id == id && x.Organization.Id == organizationId);
            if (shift == null) throw new ObjectNotFoundException("Could not find a shift corresponding to the given id");

            if(shift.CheckIns.Any()) throw new ForbiddenException("You cannot delete a shift that contains checked in employees");

            _context.Shifts.Remove(shift);
            _context.SaveChanges();
        }

        public IEnumerable<Shift> ReadFromOrganization(string organizationShortKey)
        {
            return _context.Shifts
                .Include(x => x.CheckIns)
                .Include(x => x.CheckIns.Select(y => y.Employee))
                .Include(x => x.Employees)
                .Include(x => x.Employees.Select(y => y.EmployeeTitle))
                .Where(x => x.Organization.ShortKey == organizationShortKey);
        }

        public Shift Read(int id, int organizationId)
        {
            return _context.Shifts.FirstOrDefault(x => x.Id == id && x.Organization.Id == organizationId);
        }

        public IEnumerable<Shift> ReadFromOrganization(int organizationId)
        {
            return _context.Shifts
                .Include(x => x.CheckIns)
                .Include(x => x.CheckIns.Select(y => y.Employee))
                .Include(x => x.Employees)
                .Include(x => x.Employees.Select(y => y.EmployeeTitle))
                .Include(x => x.Employees.Select(y => y.Photo))
                .Where(x => x.Organization.Id == organizationId);
        }

        public int Update(Shift shift)
        {
            var dbShift = _context.Shifts.Single(s => shift.Id == s.Id);

            dbShift.Start = shift.Start;
            dbShift.End = shift.End;
            dbShift.Employees = shift.Employees.Select(e => _context.Employees.Single(em => em.Id == e.Id)).ToList();
            dbShift.CheckIns = shift.CheckIns;

            return _context.SaveChanges();
        }

        public IEnumerable<Shift> GetOngoingShifts(int organizationId, DateTime now)
        {
            var inOneHour = now.AddHours(1);
            return _context.Shifts
                .Where(s => s.Organization.Id == organizationId && (s.Start < now && s.End > now || (s.Start > now && s.Start < inOneHour)));
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}