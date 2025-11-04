using EmployeesStructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EmployeesStructure.Data.Repositories
{
    public class VacationRepository : Repository<Vacation>, IVacationRepository
    {
        public VacationRepository(DataBaseContext dbContext) : base(dbContext)
        {
        }

        public List<Vacation> GetEmployeeVacationStats(int employeeId, DateTime today)
        {
            var currentYear = today.Year;
            var yearStart = new DateTime(currentYear, 1, 1);
            var yearBack = new DateTime(currentYear - 1, 1, 1);
            var yearEnd = new DateTime(currentYear + 1, 1, 1);

            var vacations = _dbSet
                .Where(v => v.EmployeeId == employeeId
                    && v.DateSience < today
                    && (
                    (v.DateUntil >= yearStart
                    && v.DateUntil < yearEnd)
                    || (v.DateUntil >= yearBack
                    && v.DateUntil < yearEnd)
                    ))
               .ToList();

            return vacations;
        }
    }
}