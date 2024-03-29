using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using APICatalogo.Context;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Repositories
{
    public class Repository<T> : IRepository<T>  where T : class
    {
        protected readonly AppDbContext _context;

        public Repository(AppDbContext contexto)
        {
            _context = contexto;
        }

        async public Task<T?> CreateAsync(T entity)
        {
            _context.Set<T>().Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        async public Task<T?> DeleteAsync(T entity)
        {
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        async public Task<IEnumerable<T>> GetAllAsync(int page, int size)
        {
            return await _context.Set<T>().Skip(page).Take(size).ToListAsync();
        }
        

        async public Task<T?> GetAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().FirstOrDefaultAsync(predicate);
        }

        async public Task<T?> UpdateAsync(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}