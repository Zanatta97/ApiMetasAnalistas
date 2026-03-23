using ApiMetasAnalistas.Models;
using System.Linq.Expressions;

namespace ApiMetasAnalistas.Interfaces
{
    public interface IRepository<T> where T : class
    {
        public IEnumerable<T> GetAll();
        public T? Get(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Retorna o analista sem Tracking
        /// Melhora o desempenho quando não há necessidade de alteração do objeto
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public T? GetReadOnly(Expression<Func<T, bool>> predicate);
        public void Add(T entity);
        public void Update(T entity);
        public void Delete(T entity);
    }
}
