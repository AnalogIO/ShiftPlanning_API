﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ShiftPlanning.Model;
using ShiftPlanning.Model.Models;
using ShiftPlanning.WebApi.Exceptions;

namespace ShiftPlanning.WebApi.Repositories
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
            if (schedule == null) throw new ObjectNotFoundException("Could not find a schedule corresponding to the given id");

            if(schedule.Shifts.Any(s => s.CheckIns.Any())) throw new ForbiddenException("You cannot delete a schedule that has been rolled out and contains checkins.");

            _context.Schedules.Remove(schedule);
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public Schedule Read(int id, int organizationId)
        {
            return _context.Schedules
                .Where(x => x.Id == id && x.Organization.Id == organizationId)
                .Include(x => x.Shifts)
                .ThenInclude(shift => shift.CheckIns)
                .Include(x => x.ScheduledShifts)
                .ThenInclude(shift => shift.EmployeeAssignments)
                .ThenInclude(assignment => assignment.Employee)
                .ThenInclude(employee => employee.CheckIns)
                .Include(x => x.ScheduledShifts)
                .ThenInclude(shift => shift.EmployeeAssignments)
                .ThenInclude(assignment => assignment.Employee)
                .ThenInclude(employee => employee.Role_)
                .SingleOrDefault();
        }

        public IEnumerable<Schedule> ReadFromOrganization(int organizationId)
        {
            return _context.Schedules
                .Where(x => x.Organization.Id == organizationId)
                .Include(x => x.ScheduledShifts)
                .ThenInclude(shift => shift.EmployeeAssignments)
                .ThenInclude(assignment => assignment.Employee)
                .ThenInclude(employee => employee.CheckIns)
                .Include(x => x.ScheduledShifts)
                .ThenInclude(shift => shift.EmployeeAssignments)
                .ThenInclude(assignment => assignment.Employee)
                .ThenInclude(employee => employee.Role_)
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
            var schedule = _context.Schedules
                .Include(x => x.ScheduledShifts)
                .SingleOrDefault(x => x.Id == scheduleId && x.Organization.Id == organizationId);
            if (schedule == null) throw new ObjectNotFoundException("Could not find a schedule corresponding to the given id");

            var scheduledShift = schedule.ScheduledShifts.SingleOrDefault(x => x.Id == scheduledShiftId);
            if(scheduledShift == null) throw new ObjectNotFoundException("Could not find a scheduledshift corresponding to the given id of the given schedule");

            schedule.ScheduledShifts.Remove(scheduledShift);

            _context.SaveChanges();

        }

        public IEnumerable<ScheduledShift> GetScheduledShifts(IEnumerable<int> scheduledShiftIds)
        {
            return _context.ScheduledShifts.Where(x => scheduledShiftIds.Contains(x.Id));
        }

        public void DeletePreferences(IEnumerable<Preference> preferences)
        {
            _context.Preferences.RemoveRange(preferences);
            _context.SaveChanges();
        }

        public void DeletePreference(Preference preference)
        {
            _context.Preferences.Remove(preference);
            _context.SaveChanges();
        }
    }
}