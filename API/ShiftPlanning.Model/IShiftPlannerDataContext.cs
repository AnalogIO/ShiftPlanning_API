using System;
using Microsoft.EntityFrameworkCore;
using ShiftPlanning.Model.Models;

namespace ShiftPlanning.Model
{
    public interface IShiftPlannerDataContext : IDisposable
    {
        DbSet<Employee> Employees { get; set; }
        DbSet<Photo> Photos { get; set; }
        DbSet<Schedule> Schedules { get; set; }
        DbSet<Organization> Organizations { get; set; }
        DbSet<Shift> Shifts { get; set; }
        DbSet<CheckIn> CheckIns { get; set; }
        DbSet<ScheduledShift> ScheduledShifts { get; set; }
        DbSet<Role> Roles { get; set; }
        DbSet<Preference> Preferences { get; set; }
        DbSet<Friendship> Friendships { get; set; }

        int SaveChanges();
    }
}