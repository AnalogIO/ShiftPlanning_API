using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using API.Models;

namespace API.Data
{
    public class ShiftRepository : IShiftRepository, IDisposable
    {
        private IShiftPlannerDataContext _context;

        public ShiftRepository()
        {
            _context = new ShiftPlannerDataContext();
        }

        public ShiftRepository(ShiftPlannerDataContext context)
        {
            _context = context;
        }

        public Shift Create(Shift shift)
        {
            _context.Shifts.Add(shift);
            return _context.SaveChanges() > 0 ? shift : null;
        }

        public List<Shift> Create(List<Shift> shifts)
        {
            _context.Shifts.AddRange(shifts);
            return _context.SaveChanges() > 0 ? shifts : null;
        }

        public void Delete(int id, int institutionId)
        {
            var shift = _context.Shifts.Where(x => x.Id == id && x.Institution.Id == institutionId).FirstOrDefault();
            if(shift != null)
            {
                _context.Shifts.Remove(shift);
                _context.SaveChanges();
            }
        }

        public Shift Read(int id, int institutionId)
        {
            return _context.Shifts.Where(x => x.Id == id && x.Institution.Id == institutionId).FirstOrDefault();
        }

        public List<Shift> ReadFromInstitution(int institutionId)
        {
            return _context.Shifts.Where(x => x.Institution.Id == institutionId).ToList();
        }

        public int Update(Shift shift)
        {
            _context.Shifts.Attach(shift);
            _context.MarkAsModified(shift);
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}