using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Responses;
using APICatalogo.Repositories.ProductRepository;
using APICatalogo.Repositories;

namespace APICatalogo.Controllers;

    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IRepository<Product> _repository;

        public ProductsController(IRepository<Product> repository)
        {
            _repository = repository;
        }

        // GET: api/Produtos
        [HttpGet]
        public async Task<ActionResult<PaginatedResponse<Product>>> GetProducts(int pageNumber = 1, int pageSize = 10)
        {
            
                var totalRecords = await _repository.CountAsync();
                var totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);

                // Validar e ajustar os parâmetros da página
                pageNumber = Math.Max(1, pageNumber);
                pageSize = Math.Max(1, Math.Min(pageSize, 100)); // Limita o tamanho máximo da página a 100

                var Products = await _repository.GetAllAsync(pageNumber, pageSize);

                // Construir o URL para a próxima página
                string? nextPageUrl = null;
                if (pageNumber < totalPages)
                {
                    nextPageUrl = Url.Action("GetProdutos", new { pageNumber = pageNumber + 1, pageSize = pageSize });
                }

                var response = new PaginatedResponse<Product>(Products, pageNumber, pageSize, totalRecords, nextPageUrl);
                return Ok(response);
        }


        // GET: api/Produtoes/5
        [HttpGet("{id:int:min(1)}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
        
            var product = await _repository.GetAsync(c => c.ProductId == id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        // PUT: api/Produtoes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, [FromBody] Product product)
        {

            if (id != product.ProductId)
            {
                return BadRequest();
            }

            if (!await _repository.ExistsAsync(c => c.ProductId == id))
            {
                return NotFound();
            }

            await _repository.UpdateAsync(product);
            
            return NoContent();
        }

        // POST: api/Produtoes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {

            await _repository.CreateAsync(product);
            Console.WriteLine("bbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbb");

            return CreatedAtAction("PostProduct", new { id = product.ProductId }, product);

        }

        // DELETE: api/Produtoes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {   
            if (!await _repository.ExistsAsync(c => c.ProductId == id))
            {
                return NotFound();
            }


            await _repository.DeleteAsync(c => c.ProductId == id);

            return NoContent();

        }

        

    }