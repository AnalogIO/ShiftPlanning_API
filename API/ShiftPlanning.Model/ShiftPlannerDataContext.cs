using Microsoft.EntityFrameworkCore;
using ShiftPlanning.Common.Configuration;
using ShiftPlanning.Model.Models;

namespace ShiftPlanning.Model
{
    public class ShiftPlannerDataContext : DbContext, IShiftPlannerDataContext
    {
        private readonly DatabaseSettings _databaseSettings;

        public ShiftPlannerDataContext(DbContextOptions<ShiftPlannerDataContext> options,
            DatabaseSettings databaseSettings) : base(options)
        {
            _databaseSettings = databaseSettings;
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Shift> Shifts { get; set; }
        public DbSet<CheckIn> CheckIns { get; set; }
        public DbSet<ScheduledShift> ScheduledShifts { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Preference> Preferences { get; set; }
        public DbSet<Friendship> Friendships { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)
            );
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(_databaseSettings.SchemaName);
            modelBuilder.Entity<Role>()
                .HasMany(left => left.Employee_)
                .WithMany(right => right.Role_)
                .UsingEntity(join => join.ToTable("RoleEmployees"));
            modelBuilder.Entity<Shift>()
                .HasMany(left => left.Employee_)
                .WithMany(right => right.Shift_)
                .UsingEntity(join => join.ToTable("ShiftEmployees"));
        }
    }
}