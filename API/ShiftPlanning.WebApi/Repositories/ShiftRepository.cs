using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ShiftPlanning.Model;
using ShiftPlanning.Model.Models;
using ShiftPlanning.WebApi.Exceptions;

namespace ShiftPlanning.WebApi.Repositories
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
            var shifts = _context.Shifts
                .Where(x => x.Organization.ShortKey == organizationShortKey)
                .Include(x => x.Employee_).Include(x => x.Organization)
                .Include(x => x.Schedule).Include(x => x.CheckIns);
            return shifts;
        }

        public Shift Read(int id, int organizationId)
        {
            return _context.Shifts.Where(x => x.Id == id && x.Organization.Id == organizationId)
                .Include(x => x.Employee_).ThenInclude(e => e.Role_)
                .Include(x => x.Employee_).ThenInclude(e => e.CheckIns)
                .Include(x => x.Schedule)
                .Include(x => x.CheckIns)
                .AsSplitQuery()
                .FirstOrDefault();
        }

        public IEnumerable<Shift> ReadFromOrganization(int organizationId)
        {
            return _context.Shifts
                .Where(x => x.Organization.Id == organizationId)
                .Include(x => x.Employee_).ThenInclude(e => e.Role_)
                .Include(x => x.Employee_).ThenInclude(e => e.CheckIns)
                .Include(x => x.CheckIns)
                .Include(x => x.Schedule)
                .AsSplitQuery()
                .ToList();
        }

        public int Update(Shift shift)
        {
            var dbShift = _context.Shifts.Single(s => shift.Id == s.Id);

            dbShift.Start = shift.Start;
            dbShift.End = shift.End;
            dbShift.Employee_ = shift.Employee_.Select(e => _context.Employees.Single(em => em.Id == e.Id)).ToList();
            dbShift.CheckIns = shift.CheckIns;

            return _context.SaveChanges();
        }

        public IEnumerable<Shift> GetOngoingShifts(int organizationId, DateTime now)
        {
            var inOneHour = now.AddHours(1);
            return _context.Shifts
                .Where(s => s.Organization.Id == organizationId && (s.Start < now && s.End > now || (s.Start > now && s.Start < inOneHour)));
        }

        public bool IsOrganisationOpen(string shortKey)
        {
             var now = DateTime.Now;
 
             return _context.Shifts
                 .Any(shift => shift.Organization.ShortKey == shortKey && shift.Start <= now && now <= shift.End && shift.CheckIns.Any());
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}