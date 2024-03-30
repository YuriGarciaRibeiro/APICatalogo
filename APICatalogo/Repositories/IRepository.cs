using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace APICatalogo.Repositories
{
    public interface IRepository<T>
    {
        Task<IEnumerable<T>> GetAllAsync(int page, int size);
        Task<T?> GetAsync(Expression<Func<T, bool>> predicate);
        Task<T?> CreateAsync(T entity);
        Task<T?> UpdateAsync(T entity);
        Task<T?> DeleteAsync(Expression<Func<T, bool>> predicate);
        Task<int> CountAsync();
        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);
    }
}