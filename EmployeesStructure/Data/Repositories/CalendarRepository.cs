using EmployeesStructure.Models;
using System.Collections.Generic;
using System.Linq;

namespace EmployeesStructure.Data.Repositories
{
    public class CalendarRepository : Repository<Calendar>, ICalendarRepository
    {
        public CalendarRepository(DataBaseContext dbContext) : base(dbContext)
        {
        }

        public IEnumerable<Calendar> GetAllForYear(int year)
        {
            return _dbSet.Where(c => c.Date.Year == year).ToList();
        }
    }
}