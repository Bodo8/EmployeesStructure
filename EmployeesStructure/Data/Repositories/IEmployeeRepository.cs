using EmployeesStructure.Models;
using System.Linq;

namespace EmployeesStructure.Data.Repositories
{
    public interface IEmployeeRepository : IRepository<Employee>
    {
        Employee GetByIdEager(int employeeId);
        IQueryable<Employee> GetSuperiorById(int superiorId);
    }
}
