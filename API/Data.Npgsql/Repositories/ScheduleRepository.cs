using System;
using System.Collections.Generic;
using System.Linq;
using Data.Models;
using Data.Repositories;

namespace Data.Npgsql.Repositories
{
    public class ScheduleRepository : IScheduleRepository, IDisposable
    {
        private readonly IShiftPlannerDataContext _context;

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
            var schedule = _context.Schedules.SingleOrDefault(x => x.Id == id && x.Institution.Id == institutionId);
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
            return _context.Schedules.SingleOrDefault(x => x.Id == id && x.Institution.Id == institutionId);
        }

        public IEnumerable<Schedule> ReadFromInstitution(int institutionId)
        {
            return _context.Schedules.Where(x => x.Institution.Id == institutionId);
        }

        public int Update(Schedule schedule)
        {
            var pgSchedule = _context.Schedules.Single(s => s.Id == schedule.Id);

            pgSchedule.Name = schedule.Name;
            pgSchedule.NumberOfWeeks = schedule.NumberOfWeeks;
            pgSchedule.ScheduledShifts = schedule.ScheduledShifts;
            
            return _context.SaveChanges();
        }
    }
}