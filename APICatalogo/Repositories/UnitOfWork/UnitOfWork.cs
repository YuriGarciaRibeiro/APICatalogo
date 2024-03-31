using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APICatalogo.Context;
using APICatalogo.Repositories.CategoryRepositoryGroup;
using APICatalogo.Repositories.ProductRepositoryGroup;


namespace APICatalogo.Repositories.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private IProductRepository? _productRepository;

        private ICategoryRepository? _categoryRepository;

        private readonly AppDbContext _context;

        
        public UnitOfWork(AppDbContext contexto)
        {

            _context = contexto;
        }
        

        public IProductRepository ProductRepository
        {
            get
            {
                return _productRepository = _productRepository ?? new ProductRepository(_context);
            }
        }

        public ICategoryRepository CategoryRepository
        {
            get
            {
                return _categoryRepository = _categoryRepository ?? new CategoryRepository(_context);
            }
        }

        public Task CommitAsync()
        {
            return _context.SaveChangesAsync();
        }

        public void DisposeAsync()
        {
            _context.DisposeAsync();
        }
    }
}