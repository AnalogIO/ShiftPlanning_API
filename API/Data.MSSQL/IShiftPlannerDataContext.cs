using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using Data.Models;

namespace Data.MSSQL
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
        DbEntityEntry Entry(object entity);
    }
}