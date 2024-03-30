using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using APICatalogo.Context;
using APICatalogo.Exceptions;
using APICatalogo.Migrations;
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
            try
            {
                Console.WriteLine("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");
                _context.Set<T>().Add(entity);
                Console.WriteLine("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");

                await _context.SaveChangesAsync();
                Console.WriteLine("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");

                return entity;
            }
            catch (Exception ex)
            {
                
                throw new CreateEntityException("Erro ao criar entidade", ex);
            }
        }

        async public Task<T?> DeleteAsync(Expression<Func<T, bool>> predicate)
        {
            try
            {
                var entity = await _context.Set<T>().SingleOrDefaultAsync(predicate);
                if (entity == null)
                {
                    return null;
                }
                _context.Set<T>().Remove(entity);
                await _context.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new DeleteEntityException("Erro ao deletar entidade", ex);
            }
        }

        async public Task<IEnumerable<T>> GetAllAsync(int page, int size)
        {
            try {
                return await _context.Set<T>().AsNoTracking()                      
                                             .Skip((page - 1) * size)
                                             .Take(size)
                                             .ToListAsync();
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new InternalServerErrorException("Erro ao buscar entidades", ex);
            }
        }
        

        async public Task<T?> GetAsync(Expression<Func<T, bool>> predicate)
        {
            try {
                return await _context.Set<T>().SingleOrDefaultAsync(predicate);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new InternalServerErrorException("Erro ao buscar entidade", ex);
            }
        }

        async public Task<T?> UpdateAsync(T entity)
        {
            try
            {
                _context.Entry(entity).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new UpdateEntityException("Erro ao atualizar entidade", ex);
            }
        }

        async public Task<int> CountAsync()
        {
            try
            {
                return await _context.Set<T>().CountAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new InternalServerErrorException("Erro ao contar entidades", ex);
            }
        }

        async public Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
        {
            try
            {
                return await _context.Set<T>().AnyAsync(predicate);
            }
            catch (Exception ex)
            {
                
                throw new InternalServerErrorException("Erro ao verificar existência de entidade", ex);
            }
        }
    }
}