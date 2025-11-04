namespace EmployeesStructure.Migrations
{
    using EmployeesStructure.Models;
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<EmployeesStructure.Data.DataBaseContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(EmployeesStructure.Data.DataBaseContext context)
        {
            context.Employees.AddOrUpdate(
                e => e.Id,
               new Employee { Id = 1, Name = "Jan Kowalski", SuperiorId = null, TeamId = 1, VacationPackageId = 1 },
               new Employee { Id = 2, Name = "Kamil Nowak", SuperiorId = 1, TeamId = 1, VacationPackageId = 1 },
               new Employee { Id = 3, Name = "Anna Lewandowska", SuperiorId = 1, TeamId = 1, VacationPackageId = 1 },
               new Employee { Id = 4, Name = "Andrzej Abacki", SuperiorId = 2, TeamId = 1, VacationPackageId = 1 },
               new Employee { Id = 5, Name = "Piotr Wiśniewski", SuperiorId = 2, TeamId = 1, VacationPackageId = 1 },
               new Employee { Id = 6, Name = "Maria Dąbrowska", SuperiorId = 3, TeamId = 1, VacationPackageId = 1 }
            );

            context.SaveChanges();
        }
    }
}
