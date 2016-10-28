using System;
using System.Data.Entity;
using API.Models;

namespace API.Data
{
    public class ShiftPlannerDataContext : DbContext, IShiftPlannerDataContext
    {
        public ShiftPlannerDataContext(): base("name=ShiftPlannerDataContext") {

        }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<EmployeeTitle> EmployeeTitles { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<Institution> Institutions { get; set; }
        public DbSet<Manager> Managers { get; set; }
        public void MarkAsModified(Employee employee)
        {
            Entry(employee).State = EntityState.Modified;
        }
        public void MarkAsModified(EmployeeTitle employeeTitle)
        {
            Entry(employeeTitle).State = EntityState.Modified;
        }

        public void MarkAsModified(Institution institution)
        {
            Entry(institution).State = EntityState.Modified;
        }

        public void MarkAsModified(Manager manager)
        {
            Entry(manager).State = EntityState.Modified;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // PostgreSQL uses the public schema by default - not dbo. 
            modelBuilder.HasDefaultSchema("public");
            base.OnModelCreating(modelBuilder);
        }
    }
}