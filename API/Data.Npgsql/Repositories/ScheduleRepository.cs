using System;
using System.Collections.Generic;
using System.Linq;
using Data.Models;
using Data.Repositories;
using System.Data.Entity;

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

        public void Delete(int id, int organizationId)
        {
            var schedule = _context.Schedules.SingleOrDefault(x => x.Id == id && x.Organization.Id == organizationId);
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

        public Schedule Read(int id, int organizationId)
        {
            return _context.Schedules
                .Include(x => x.ScheduledShifts)
                .Include(x => x.ScheduledShifts.Select(y => y.Employees))
                .Include(x => x.ScheduledShifts.Select(y => y.Employees.Select(z => z.EmployeeTitle)))
                .SingleOrDefault(x => x.Id == id && x.Organization.Id == organizationId);
        }

        public IEnumerable<Schedule> ReadFromOrganization(int organizationId)
        {
            return _context.Schedules
                .Include(x => x.ScheduledShifts)
                .Include(x => x.ScheduledShifts.Select(y => y.Employees))
                .Include(x => x.ScheduledShifts.Select(y => y.Employees.Select(z => z.EmployeeTitle)))
                .Where(x => x.Organization.Id == organizationId)
                .ToList();
        }

        public int Update(Schedule schedule)
        {
            var pgSchedule = _context.Schedules.Single(s => s.Id == schedule.Id);

            pgSchedule.Name = schedule.Name;
            pgSchedule.NumberOfWeeks = schedule.NumberOfWeeks;
            pgSchedule.ScheduledShifts = schedule.ScheduledShifts;
            
            return _context.SaveChanges();
        }

        public void DeleteScheduledShift(int scheduleId, int scheduledShiftId, int organizationId)
        {
            var schedule = _context.Schedules.SingleOrDefault(x => x.Id == scheduleId && x.Organization.Id == organizationId);
            if (schedule == null) return;

            var scheduledShift = schedule.ScheduledShifts.SingleOrDefault(x => x.Id == scheduledShiftId);
            schedule.ScheduledShifts.Remove(scheduledShift);

            _context.SaveChanges();

        }
    }
}