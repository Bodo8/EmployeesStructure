using EmployeesStructure.Models;
using System.Data.Entity;
using System.Linq;

namespace EmployeesStructure.Data.Repositories
{
    public class EmployeeRepository : Repository<Models.Employee>, IEmployeeRepository
    {
        public EmployeeRepository(DataBaseContext dbContext) : base(dbContext)
        {
        }

        public Employee GetByIdEager(int employeeId)
        {
            return _dbSet.Where(e => e.Id == employeeId)
                .Include(e => e.VacationPackage)
                .FirstOrDefault();
        }

        public IQueryable<Employee> GetSuperiorById(int superiorId)
        {
            return _dbSet
                .Where(e => e.SuperiorId == superiorId);
        }
    }
}