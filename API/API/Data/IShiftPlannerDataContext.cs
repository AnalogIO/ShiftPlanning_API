using API.Models;
using System;
using System.Data.Entity;

namespace API.Data
{
    public interface IShiftPlannerDataContext : IDisposable
    {
        DbSet<Employee> Employees { get; set; }
        DbSet<EmployeeTitle> EmployeeTitles { get; set; }
        DbSet<Schedule> Schedules { get; set; }
        DbSet<Institution> Institutions { get; set; }
        DbSet<Manager> Managers { get; set; }
        DbSet<Shift> Shifts { get; set; }
        int SaveChanges();
        void MarkAsModified(Employee employee);
        void MarkAsModified(EmployeeTitle employeeTitle);
        void MarkAsModified(Institution institution);
        void MarkAsModified(Manager manager);
        void MarkAsModified(Shift shift);
    }
}