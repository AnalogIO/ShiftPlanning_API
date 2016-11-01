using System;
using System.Collections.Generic;
using System.Linq;
using Data.Models;
using Data.Npgsql.Mapping;
using Data.Repositories;
using PGShift = Data.Npgsql.Models.Shift;
using PGCheckIn = Data.Npgsql.Models.CheckIn;

namespace Data.Npgsql.Repositories
{
    public class ShiftRepository : IShiftRepository, IDisposable
    {
        private readonly IShiftPlannerDataContext _context;
        private readonly IMapper<Shift, PGShift> _shiftMapper;
        private readonly IMapMany<PGShift, Shift> _shiftMapMany;

        public ShiftRepository(IShiftPlannerDataContext context, 
            IMapper<Shift, PGShift> shiftMapper, 
            IMapMany<PGShift, Shift> shiftMapMany)
        {                                                        
            _context = context;
            _shiftMapper = shiftMapper;
            _shiftMapMany = shiftMapMany;
        }

        public Shift Create(Shift shift)
        {
            var newShift = _shiftMapper.MapToEntity(shift);
            
            _context.Shifts.Add(newShift);
            return _context.SaveChanges() > 0 ? _shiftMapper.MapToModel(newShift) : null;
        }

        public IEnumerable<Shift> Create(ICollection<Shift> shifts)
        {
            var newShifts = shifts.Select(_shiftMapper.MapToEntity).ToList();

            _context.Shifts.AddRange(newShifts);
            return _context.SaveChanges() > 0 ? _shiftMapMany.Map(newShifts) : null;
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
            return _shiftMapper.MapToModel(_context.Shifts.FirstOrDefault(x => x.Id == id && x.Institution.Id == institutionId));
        }

        public IEnumerable<Shift> ReadFromInstitution(int institutionId)
        {
            return _shiftMapMany.Map(_context.Shifts.Where(x => x.Institution.Id == institutionId));
        }

        public int Update(Shift shift)
        {
            var dbShift = _context.Shifts.Single(s => shift.Id == s.Id);

            dbShift.Start = shift.Start;
            dbShift.End = shift.End;
            dbShift.Employees = shift.Employees.Select(e => _context.Employees.Single(em => em.Id == e.Id)).ToList();
            dbShift.CheckIns = shift.CheckIns.Select(ci => new PGCheckIn
            {
                Employee = _context.Employees.Single(e => e.Id == ci.Employee.Id),
                Time = ci.Time
            }).ToList();
            
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}