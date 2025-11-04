using EmployeesStructure.Models;
using System;
using System.Collections.Generic;

namespace EmployeesStructure.Data.Repositories
{
    public interface IVacationRepository : IRepository<Vacation>
    {
        List<Vacation> GetEmployeeVacationStats(int employeeId, DateTime today);
    }
}