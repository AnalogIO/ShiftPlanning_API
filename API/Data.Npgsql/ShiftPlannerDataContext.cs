using System.Data.Entity;
using Data.Models;

namespace Data.Npgsql
{
    public class ShiftPlannerDataContext : DbContext, IShiftPlannerDataContext
    {
        public ShiftPlannerDataContext(): base("name=ShiftPlannerDataContext") {

        }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<EmployeeTitle> EmployeeTitles { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Manager> Managers { get; set; }
        public DbSet<Shift> Shifts { get; set; }
        public DbSet<CheckIn> CheckIns { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // PostgreSQL uses the public schema by default - not dbo. 
            modelBuilder.HasDefaultSchema("public");

            modelBuilder.Entity<Organization>()
                .HasKey(org => org.Id);

            modelBuilder.Entity<Organization>()
                .HasOptional(org => org.DefaultPhoto);

            modelBuilder.Entity<Employee>()
                .HasKey(emp => emp.Id)
                .HasOptional(emp => emp.Photo);

            modelBuilder.Entity<Photo>()
                .HasKey(photo => photo.Id);

            modelBuilder.Entity<Photo>()
                .HasRequired(photo => photo.Organization)
                .WithMany(org => org.Photos);

            base.OnModelCreating(modelBuilder);
        }
    }
}