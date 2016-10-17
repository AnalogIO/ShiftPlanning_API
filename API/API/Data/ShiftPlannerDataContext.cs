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
        public DbSet<Schedule> Schedules { get; set; }
        public void MarkAsModified(Employee employee)
        {
            Entry(employee).State = EntityState.Modified;
        }
    }
}