using API.Models;
using System;
using System.Data.Entity;

namespace API.Data
{
    public interface IShiftPlannerDataContext : IDisposable
    {
        DbSet<Employee> Employees { get; set; }
        int SaveChanges();
        void MarkAsModified(Employee employee);
    }
}