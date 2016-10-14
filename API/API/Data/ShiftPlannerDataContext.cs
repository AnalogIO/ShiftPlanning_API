using System;
using System.Data.Entity;
using API.Models;

namespace API.Data
{
    public class ShiftPlannerDataContext : DbContext, IShiftPlannerDataContext
    {
        public ShiftPlannerDataContext(): base("name=ShiftPlannerDataContext") {

        }
        public DbSet<User> Users { get; set; }
    }
}