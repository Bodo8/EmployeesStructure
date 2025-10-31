using EmployeesStructure.Models;
using System.Data.Entity;

namespace EmployeesStructure.Data
{
    public class EmployeeContext : DbContext
    {
        public EmployeeContext() : base("name=DefaultConnection")
        {
            Configuration.LazyLoadingEnabled = true;
            Configuration.ProxyCreationEnabled = true;
        }

        public DbSet<Employee> Employees { get; set; }

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