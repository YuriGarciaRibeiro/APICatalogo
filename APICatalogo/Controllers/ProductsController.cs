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

namespace APICatalogo.Controllers;

    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Produtos
        [HttpGet]
        public async Task<ActionResult<PaginatedResponse<Product>>> GetProdutos(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var totalRecords = await _context.Products.CountAsync();
                var totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);

                // Validar e ajustar os parâmetros da página
                pageNumber = Math.Max(1, pageNumber);
                pageSize = Math.Max(1, Math.Min(pageSize, 100)); // Limita o tamanho máximo da página a 100

                var Products = await _context.Products
                                             .AsNoTracking()     
                                             .Include(p => p.Category)                  
                                             .Skip((pageNumber - 1) * pageSize)
                                             .Take(pageSize)
                                             .ToListAsync();

                // Construir o URL para a próxima página
                string? nextPageUrl = null;
                if (pageNumber < totalPages)
                {
                    nextPageUrl = Url.Action("GetProdutos", new { pageNumber = pageNumber + 1, pageSize = pageSize });
                }

                var response = new PaginatedResponse<Product>(Products, pageNumber, pageSize, totalRecords, nextPageUrl);

                    return Ok(response);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse { 
                StatusCode = StatusCodes.Status500InternalServerError, 
                Message = "Erro ao tentar obter os produtos do banco de dados" 
            });
            }
        }


        // GET: api/Produtoes/5
        [HttpGet("{id:int:min(1)}")]
        public async Task<ActionResult<Product>> GetProduto(int id)
        {
            try
            {
                var product = await _context.Products.AsNoTracking().FirstOrDefaultAsync(a => a.ProductId == id);

                if (product == null)
                {
                    return NotFound();
                }

                    return product;
                }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse { 
                StatusCode = StatusCodes.Status500InternalServerError, 
                Message = "Erro ao tentar obter o produto do banco de dados" 
            }); 
            }
        }

        // PUT: api/Produtoes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduto(int id, Product product)
        {
            try
            {
                if (id != product.ProductId)
                {
                    return BadRequest();
                }

                _context.Entry(product).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProdutoExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return NoContent();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse { 
                StatusCode = StatusCodes.Status500InternalServerError, 
                Message = "Erro ao tentar Atualizar o produto do banco de dados" 
            });
            }
        }

        // POST: api/Produtoes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduto(Product product)
        {
            try
            {
                _context.Products.Add(product);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetProduto", new { id = product.ProductId }, product);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse { 
                StatusCode = StatusCodes.Status500InternalServerError, 
                Message = "Erro ao Criar Novo Produto No Banco de Dados" 
            });
            }
        }

        // DELETE: api/Produtoes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduto(int id)
        {   
            try
            {
                var produto = await _context.Products.FindAsync(id);
                if (produto == null)
                {
                    return NotFound();
                }

                _context.Products.Remove(produto);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse { 
                StatusCode = StatusCodes.Status500InternalServerError, 
                Message = "Erro ao Deletar Produto Do Banco de Dados" 
            });
            }
        }

        private bool ProdutoExists(int id)
        {
            return _context.Products.Any(e => e.ProductId == id);
        }
    }