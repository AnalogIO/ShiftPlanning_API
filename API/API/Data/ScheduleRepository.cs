using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using API.Models;

namespace API.Data
{
    public class ScheduleRepository : IScheduleRepository, IDisposable
    {
        private IShiftPlannerDataContext _context;

        public ScheduleRepository()
        {
            _context = new ShiftPlannerDataContext();
        }

        public ScheduleRepository(IShiftPlannerDataContext context)
        {
            _context = context;
        }

        public Schedule Create(Schedule schedule)
        {
            _context.Schedules.Add(schedule);
            return _context.SaveChanges() > 0 ? schedule : null;
        }

        public void Delete(int id, int institutionId)
        {
            var schedule = _context.Schedules.Where(x => x.Id == id && x.Institution.Id == institutionId).FirstOrDefault();
            if(schedule != null)
            {
                _context.Schedules.Remove(schedule);
                _context.SaveChanges();
            }
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public Schedule Read(int id, int institutionId)
        {
            return _context.Schedules.Where(x => x.Id == id && x.Institution.Id == institutionId).FirstOrDefault();
        }

        public List<Schedule> ReadFromInstitution(int institutionId)
        {
            return _context.Schedules.Where(x => x.Institution.Id == institutionId).ToList();
        }

        public int Update(Schedule schedule)
        {
            _context.Schedules.Attach(schedule);
            _context.MarkAsModified(schedule);
            return _context.SaveChanges();
        }
    }
}