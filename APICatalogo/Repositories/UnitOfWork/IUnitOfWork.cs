using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APICatalogo.Repositories.CategoryRepositoryGroup;
using APICatalogo.Repositories.ProductRepositoryGroup;

namespace APICatalogo.Repositories.UnitOfWork
{
    public interface IUnitOfWork
    {
        IProductRepository ProductRepository { get; }
        ICategoryRepository CategoryRepository { get; }

        Task CommitAsync();
    }
}