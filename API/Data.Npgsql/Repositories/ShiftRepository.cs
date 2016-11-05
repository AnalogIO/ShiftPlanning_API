using System;
using System.Collections.Generic;
using System.Linq;
using Data.Models;
using Data.Repositories;

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

        public void Delete(int id, int institutionId)
        {
            var shift = _context.Shifts.FirstOrDefault(x => x.Id == id && x.Institution.Id == institutionId);
            if(shift != null)
            {
                _context.Shifts.Remove(shift);
                _context.SaveChanges();
            }
        }

        public Shift Read(int id, int institutionId)
        {
            return _context.Shifts.FirstOrDefault(x => x.Id == id && x.Institution.Id == institutionId);
        }

        public IEnumerable<Shift> ReadFromInstitution(int institutionId)
        {
            return _context.Shifts.Where(x => x.Institution.Id == institutionId);
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

        public IEnumerable<Shift> GetOngoingShifts(int institutionId, DateTime now)
        {
            var inOneHour = now.AddHours(1);
            return _context.Shifts.Where(s => s.Institution.Id == institutionId && (s.Start < now && s.End > now) || (s.Start > now && s.Start < inOneHour));
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}