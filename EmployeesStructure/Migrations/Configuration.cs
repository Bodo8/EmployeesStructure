namespace EmployeesStructure.Migrations
{
    using EmployeesStructure.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<EmployeesStructure.Data.EmployeeContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(EmployeesStructure.Data.EmployeeContext context)
        {
            context.Employees.AddOrUpdate( 
                e => e.Id, 
               new Employee { Id = 1, Name = "Jan Kowalski", SuperiorId = null },
               new Employee { Id = 2, Name = "Kamil Nowak", SuperiorId = 1 },
               new Employee { Id = 3, Name = "Anna Lewandowska", SuperiorId = 1 },
               new Employee { Id = 4, Name = "Andrzej Abacki", SuperiorId = 2 },
               new Employee { Id = 5, Name = "Piotr Wiśniewski", SuperiorId = 2 },
               new Employee { Id = 6, Name = "Maria Dąbrowska", SuperiorId = 3 }
            );

            context.SaveChanges();
        }
    }
}
