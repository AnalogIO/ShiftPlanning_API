﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Data.Models;
using Data.Repositories;
using System.Data.Entity;
using Data.Exceptions;
using System.Data.Entity.Core;

namespace Data.MSSQL.Repositories
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
                .Where(x => x.Organization.ShortKey == organizationShortKey)
                .OrderBy(s => s.Start);
        }

        public Shift Read(int id, int organizationId)
        {
            return _context.Shifts
                .FirstOrDefault(x => x.Id == id && x.Organization.Id == organizationId);
        }

        public IEnumerable<Shift> ReadFromOrganization(int organizationId)
        {
            return _context.Shifts
                .Where(x => x.Organization.Id == organizationId)
                .Include(x => x.Employees.Select(e => e.Roles))
                .OrderBy(s => s.Start);
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