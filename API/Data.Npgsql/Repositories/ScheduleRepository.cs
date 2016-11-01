using System;
using System.Collections.Generic;
using System.Linq;
using Data.Models;
using Data.Npgsql.Mapping;
using Data.Repositories;
using PGSchedule = Data.Npgsql.Models.Schedule;
using PGScheduledShift = Data.Npgsql.Models.ScheduledShift;

namespace Data.Npgsql.Repositories
{
    public class ScheduleRepository : IScheduleRepository, IDisposable
    {
        private readonly IShiftPlannerDataContext _context;
        private readonly IMapper<Schedule, PGSchedule> _scheduleMapper;
        private readonly IMapMany<PGSchedule, Schedule> _scheduleMapMany;

        public ScheduleRepository(IShiftPlannerDataContext context, 
            IMapper<Schedule, PGSchedule> scheduleMapper,
            IMapMany<PGSchedule, Schedule> scheduleMapMany)
        {
            _context = context;
            _scheduleMapper = scheduleMapper;
            _scheduleMapMany = scheduleMapMany;
        }

        public Schedule Create(Schedule schedule)
        {
            var newSchedule = _scheduleMapper.MapToEntity(schedule);
            

            _context.Schedules.Add(newSchedule);
            return _context.SaveChanges() > 0 ? _scheduleMapper.MapToModel(newSchedule) : null;
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
            return _scheduleMapper.MapToModel(_context.Schedules.SingleOrDefault(x => x.Id == id && x.Institution.Id == institutionId));
        }

        public IEnumerable<Schedule> ReadFromInstitution(int institutionId)
        {
            return _scheduleMapMany.Map(_context.Schedules.Where(x => x.Institution.Id == institutionId));
        }

        public int Update(Schedule schedule)
        {
            var pgSchedule = _context.Schedules.Single(s => s.Id == schedule.Id);

            pgSchedule.Name = schedule.Name;
            pgSchedule.NumberOfWeeks = schedule.NumberOfWeeks;
            pgSchedule.Shifts = schedule.Shifts.Select(ss => new PGScheduledShift
            {
                Day = ss.Day,
                Start = ss.Start,
                End = ss.End,
                Employees = ss.Employees.Select(e => _context.Employees.Single(em => em.Id == e.Id)).ToList()
            }).ToList();
            
            return _context.SaveChanges();
        }
    }
}