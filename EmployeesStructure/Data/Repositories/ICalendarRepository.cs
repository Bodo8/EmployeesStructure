using EmployeesStructure.Models;
using System.Collections.Generic;

namespace EmployeesStructure.Data.Repositories
{
    public interface ICalendarRepository
    {
        IEnumerable<Calendar> GetAllForYear(int year);
    }
}
