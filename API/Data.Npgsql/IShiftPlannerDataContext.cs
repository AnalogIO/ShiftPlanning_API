using System;
using System.Data.Entity;
using Data.Models;

namespace Data.Npgsql
{
    public interface IShiftPlannerDataContext : IDisposable
    {
        DbSet<Employee> Employees { get; set; }
        DbSet<EmployeeTitle> EmployeeTitles { get; set; }
        DbSet<Schedule> Schedules { get; set; }
        DbSet<Organization> Organizations { get; set; }
        DbSet<Manager> Managers { get; set; }
        DbSet<Shift> Shifts { get; set; }
        DbSet<CheckIn> CheckIns { get; set; }
        int SaveChanges();
    }
}