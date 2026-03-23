using ApiMetasAnalistas.Context;
using ApiMetasAnalistas.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ApiMetasAnalistas.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly AppDBContext _context;

        public Repository(AppDBContext context)
        {
            _context = context;
        }

        public virtual IEnumerable<T> GetAll()
        {
            return _context.Set<T>().AsNoTracking().ToList();
        }

        public virtual T? Get(Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>().FirstOrDefault(predicate);
        }

        public virtual T? GetReadOnly(Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>().AsNoTracking().FirstOrDefault(predicate);
        }

        public void Add(T entity)
        {
            _context.Set<T>().Add(entity);
        }

        public void Update(T entity)
        {
            _context.Set<T>().Update(entity);
        }

        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }
    }
}
