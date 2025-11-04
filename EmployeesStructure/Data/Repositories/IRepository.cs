using System.Collections.Generic;

namespace EmployeesStructure.Data.Repositories
{
    public interface IRepository<T> where T : class
    {
        T GetById(int id);
        void Add(T entity);
        void Update(T entity);
        IEnumerable<T> GetAll();
        void Delete(T entity);
        void Save();
    }
}
