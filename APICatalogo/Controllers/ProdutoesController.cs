using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APICatalogo.Context;
using APICatalogo.Models;

namespace APICatalogo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutoesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProdutoesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Produtos
        [HttpGet]
        public async Task<ActionResult<PaginatedResponse<Produto>>> GetProdutos(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var totalRecords = await _context.Produtos.CountAsync();
                var totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);

                // Validar e ajustar os parâmetros da página
                pageNumber = Math.Max(1, pageNumber);
                pageSize = Math.Max(1, Math.Min(pageSize, 100)); // Limita o tamanho máximo da página a 100

                var produtos = await _context.Produtos
                                             .AsNoTracking()     
                                             .Include(p => p.Categoria)                  
                                             .Skip((pageNumber - 1) * pageSize)
                                             .Take(pageSize)
                                             .ToListAsync();

                // Construir o URL para a próxima página
                string? nextPageUrl = null;
                if (pageNumber < totalPages)
                {
                    nextPageUrl = Url.Action("GetProdutos", new { pageNumber = pageNumber + 1, pageSize = pageSize });
                }

                var response = new PaginatedResponse<Produto>(produtos, pageNumber, pageSize, totalRecords, nextPageUrl);

                    return Ok(response);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                                          "Erro ao tentar obter os produtos do banco de dados");
            }
        }


        // GET: api/Produtoes/5
        [HttpGet("{id:int:min(1)}")]
        public async Task<ActionResult<Produto>> GetProduto(int id)
        {
            try
            {
                var produto = await _context.Produtos.AsNoTracking().FirstOrDefaultAsync(a => a.ProdutoId == id);

                if (produto == null)
                {
                    return NotFound();
                }

                    return produto;
                }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                       "Erro ao tentar obter os produtos do banco de dados");   
            }
        }

        // PUT: api/Produtoes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduto(int id, Produto produto)
        {
            try
            {
                if (id != produto.ProdutoId)
                {
                    return BadRequest();
                }

                _context.Entry(produto).State = EntityState.Modified;

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
                return StatusCode(StatusCodes.Status500InternalServerError,
                                                          "Erro ao tentar atualizar o produto");
            }
        }

        // POST: api/Produtoes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Produto>> PostProduto(Produto produto)
        {
            try
            {
                _context.Produtos.Add(produto);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetProduto", new { id = produto.ProdutoId }, produto);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                                                             "Erro ao tentar criar um novo produto");
            }
        }

        // DELETE: api/Produtoes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduto(int id)
        {   
            try
            {
                var produto = await _context.Produtos.FindAsync(id);
                if (produto == null)
                {
                    return NotFound();
                }

                _context.Produtos.Remove(produto);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                                                             "Erro ao tentar excluir o produto");
            }
        }

        private bool ProdutoExists(int id)
        {
            return _context.Produtos.Any(e => e.ProdutoId == id);
        }
    }
}

public class PaginatedResponse<T>
{
    public IEnumerable<T> Data { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalRecords { get; set; }
    public string? NextPageUrl { get; set; }

    public PaginatedResponse(IEnumerable<T> data, int pageNumber, int pageSize, int totalRecords, string? nextPageUrl)
    {
        Data = data;
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalRecords = totalRecords;
        NextPageUrl = nextPageUrl;
    }
}

