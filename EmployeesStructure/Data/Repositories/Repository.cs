using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace EmployeesStructure.Data.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly DataBaseContext _dbContext;
        public readonly DbSet<T> _dbSet;

        public Repository(DataBaseContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<T>();
        }

        public T GetById(int id)
        {
            return _dbSet.Find(id);
        }

        public void Add(T entity)
        {
            _dbSet.Add(entity);
        }
        public void Update(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
        }

        public IEnumerable<T> GetAll()
        {
            return _dbSet.ToList();
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }
    }
}