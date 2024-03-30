using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APICatalogo.Context;
using APICatalogo.Models;

namespace APICatalogo.Repositories.CategoryRepositoryGroup
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(AppDbContext contexto) : base(contexto)
        {
        }
    }
}