using EmployeesStructure.Models;
using System.Data.Entity;

namespace EmployeesStructure.Data
{
    public class DataBaseContext : DbContext
    {
        public DataBaseContext() : base("name=DefaultConnection")
        {
            Configuration.LazyLoadingEnabled = true;
            Configuration.ProxyCreationEnabled = true;
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Vacation> Vacations { get; set; }
        public DbSet<VacationPackage> VacationPackages { get; set; }
        public DbSet<Calendar> Calendars { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Employee>()
                .HasOptional(e => e.Superior)
                .WithMany(e => e.Subordinates)
                .HasForeignKey(e => e.SuperiorId)
                .WillCascadeOnDelete(false);
        }
    }
}