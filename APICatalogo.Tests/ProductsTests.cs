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
    public class ProductsControllerTests : IDisposable
    {
        private readonly AppDbContext _context;
        private readonly ProductsController _controller;

        public ProductsControllerTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Usa um nome de DB único para cada instância de teste
                .Options;
            _context = new AppDbContext(options);

            // Popula o banco de dados em memória
            _context.Categories.Add(new Category { Name = "Test Category", ImageUrl = "http://example.com/category.jpg" });
            _context.Products.Add(new Product { Name = "Test Product", Description = "Test Description", Price = 10.99m, ImageUrl = "http://example.com/product.jpg", Stock = 100, CategoryId = 1 });
            _context.SaveChanges();

            _controller = new ProductsController(_context);
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        [Fact]
        public async Task GetProducts_ReturnsPaginatedResponse()
        {
            var result = await _controller.GetProducts();

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var response = Assert.IsType<PaginatedResponse<Product>>(okResult.Value);

            Assert.Single(response.Data);
        }

        [Fact]
        public async Task GetProduct_ReturnsProduct()
        {
            // Arrange

            // Act
            var result = await _controller.GetProduct(1);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Product>>(result);
            var product = Assert.IsType<Product>(actionResult.Value);

            Assert.Equal("Test Product", product.Name);
        }

        [Fact]
        public async Task GetProduct_ReturnsNotFound_WhenProductDoesNotExist()
        {
            // Act
            var result = await _controller.GetProduct(999); // Assuming 999 is an ID that does not exist

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }



        [Fact]
        public async Task PostProduct_AddsProduct()
        {
            var product = new Product
            {
                Name = "New Product",
                Description = "New Description",
                Price = 15.99m,
                ImageUrl = "http://example.com/newproduct.jpg",
                Stock = 50,
                CategoryId = 1
            };

            var result = await _controller.PostProduct(product);

            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var newProduct = Assert.IsType<Product>(createdAtActionResult.Value);

            Assert.Equal("New Product", newProduct.Name);
            Assert.Equal(2, _context.Products.Count());
        }
        [Fact]
        public async Task DeleteProduct_RemovesProduct()
        {
            await _controller.DeleteProduct(1);

            Assert.Empty(_context.Products);
        }
    }
}
