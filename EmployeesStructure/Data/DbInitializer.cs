using EmployeesStructure.Models;
using System.Collections.Generic;
using System.Data.Entity;

namespace EmployeesStructure.Data
{
    public class DbInitializer : DropCreateDatabaseIfModelChanges<EmployeeContext>
    {
        protected override void Seed(EmployeeContext context)
        {
            var employees = new List<Employee>
            {
               new Employee { Name = "Jan Kowalski", SuperiorId = null },
               new Employee { Name = "Kamil Nowak", SuperiorId = 1 },
               new Employee { Name = "Anna Lewandowska", SuperiorId = 1 },
               new Employee { Name = "Andrzej Abacki", SuperiorId = 2 },
               new Employee { Name = "Piotr Wiśniewski", SuperiorId = 2 },
               new Employee { Name = "Maria Dąbrowska", SuperiorId = 3 }
            };

            context.Employees.AddRange(employees);
            context.SaveChanges();

            base.Seed(context);
        }
    }
}