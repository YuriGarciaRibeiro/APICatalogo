using APICatalogo.Context;
using APICatalogo.Controllers;
using APICatalogo.Models;
using APICatalogo.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace APICatalogo.Tests
{
    public class CategoriesControllerTests : IDisposable
    {
        private readonly AppDbContext _context;
        private readonly CategoriesController _controller;

        public CategoriesControllerTests()
        {
            // Configurar o DbContext em memória
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _context = new AppDbContext(options);

            // Popula o banco de dados em memória para testes
            _context.Categories.Add(new Category { CategoryId = 1, Name = "Test Category", ImageUrl = "http://example.com/image.jpg" });
            _context.SaveChanges();

            // Instancia o controller com o DbContext
            _controller = new CategoriesController(_context);
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        [Fact]
        public async Task GetCategories_ReturnsPaginatedResponse()
        {
            var result = await _controller.GetCategories();

            // Verifica se o resultado é OkObjectResult com PaginatedResponse<Category>
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var response = Assert.IsAssignableFrom<PaginatedResponse<Category>>(okResult.Value);

            Assert.Single(response.Data);
        }

        [Fact]
        public async Task GetCategory_ReturnsCategory()
        {
            var result = await _controller.GetCategory(1);

            // Verifica se o resultado é Category
            var actionResult = Assert.IsType<ActionResult<Category>>(result);
            var category = Assert.IsType<Category>(actionResult.Value);

            Assert.Equal("Test Category", category.Name);
        }

        [Fact]
        public async Task PostCategory_CreatesNewCategory()
        {
            var newCategory = new Category { Name = "New Category", ImageUrl = "http://example.com/newimage.jpg" };
            var result = await _controller.PostCategory(newCategory);

            // Verifica se o resultado é CreatedAtActionResult
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var category = Assert.IsType<Category>(createdAtActionResult.Value);

            Assert.Equal("New Category", category.Name);
            Assert.Equal(2, _context.Categories.Count());
        }

        [Fact]
        public async Task DeleteCategory_RemovesCategory()
        {
            var result = await _controller.DeleteCategory(1);

            // Verifica se o resultado é NoContentResult
            Assert.IsType<NoContentResult>(result);

            Assert.Empty(_context.Categories);
        }
    }
}
