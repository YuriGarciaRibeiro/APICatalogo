using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APICatalogo.Context;
using APICatalogo.Models;

namespace APICatalogo.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoriasController : ControllerBase
{
    private readonly AppDbContext? _context;

    public CategoriasController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/Categorias
    [HttpGet]
    public async Task<ActionResult<PaginatedResponse<Categoria>>> GetCategorias(int pageNumber = 1, int pageSize = 10)
    {
        try
        {
            var totalRecords = await _context.Categorias.CountAsync();
            var totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);

            // Validar e ajustar os parâmetros da página
            pageNumber = Math.Max(1, pageNumber);
            pageSize = Math.Max(1, Math.Min(pageSize, 100)); // Limita o tamanho máximo da página a 100

            var categorias = await _context.Categorias
                                           .AsNoTracking()
                                           .Include(c => c.Produtos)
                                           .Skip((pageNumber - 1) * pageSize)
                                           .Take(pageSize)
                                           .ToListAsync();

            // Construir o URL para a próxima página
            string? nextPageUrl = null;
            if (pageNumber < totalPages)
            {
                nextPageUrl = Url.Action("GetCategorias", new { pageNumber = pageNumber + 1, pageSize = pageSize });
            }

            var response = new PaginatedResponse<Categoria>(categorias, pageNumber, pageSize, totalRecords, nextPageUrl);

            return Ok(response);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                                                                                                                  "Erro ao tentar obter as categorias do banco de dados");
        }
    }


    // GET: api/Categorias/5
    [HttpGet("{id:int:min(1)}")]
    public async Task<ActionResult<Categoria>> GetCategoria(int id)
    {
        try
        {
            var categoria = await _context.Categorias.AsNoTracking().FirstOrDefaultAsync(a => a.CategoriaId == id);

            if (categoria == null)
            {
                return NotFound();
            }

            return categoria;
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                                                                                                   "Erro ao tentar obter a categoria do banco de dados");
        }
    }

    // PUT: api/Categorias/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id:int}")]
    public async Task<IActionResult> PutCategoria(int id, Categoria categoria)
    {
        try
        {
            if (id != categoria.CategoriaId)
            {
                return BadRequest();
            }

            _context.Entry(categoria).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoriaExists(id))
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
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                                                                                    "Erro ao tentar atualizar a categoria");
        }
    }

    // POST: api/Categorias
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Categoria>> PostCategoria(Categoria categoria)
    {
        try
        {
            _context.Categorias.Add(categoria);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCategoria", new { id = categoria.CategoriaId }, categoria);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                                                                                    "Erro ao tentar criar uma nova categoria");
        }
    }

    // DELETE: api/Categorias/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategoria(int id)
    {
        try
        {
            var categoria = await _context.Categorias.FindAsync(id);
            if (categoria == null)
            {
                return NotFound();
            }

            _context.Categorias.Remove(categoria);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                                                                     "Erro ao tentar deletar a categoria");
        }
    }

    private bool CategoriaExists(int id)
    {
        return _context.Categorias.Any(e => e.CategoriaId == id);
    }
}

public class PaginatedResponse<T>
{
    public IEnumerable<T> Data { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalRecords { get; set; }
    public string? NextPageUrl { get; set; }

    public PaginatedResponse(IEnumerable<T> data, int pageNumber, int pageSize, int totalRecords, string nextPageUrl)
    {
        Data = data;
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalRecords = totalRecords;
        NextPageUrl = nextPageUrl;
    }
}
