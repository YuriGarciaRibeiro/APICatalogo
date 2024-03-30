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
using APICatalogo.Repositories;

namespace APICatalogo.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoriesController : ControllerBase
{
    
    private readonly IRepository<Category> _repository;

    public CategoriesController(IRepository<Category> repository)
    {

        _repository = repository;
    }

    // GET: api/Categorias
    [HttpGet]
    public async Task<ActionResult<PaginatedResponse<Category>>> GetCategories(int pageNumber = 1, int pageSize = 10)
    {
        
            var totalRecords = await _repository.CountAsync();
            var totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);

            // Validar e ajustar os parâmetros da página
            pageNumber = Math.Max(1, pageNumber);

            pageSize = Math.Max(1, Math.Min(pageSize, 100)); // Limita o tamanho máximo da página a 100

            var categorias = await _repository.GetAllAsync(pageNumber, pageSize);

            // Construir o URL para a próxima página (caso exista)
            string? nextPageUrl = null;
            if (pageNumber < totalPages)
            {
                nextPageUrl = Url.Action("GetCategorias", new { pageNumber = pageNumber + 1, pageSize = pageSize });
            }

            var response = new PaginatedResponse<Category>(categorias, pageNumber, pageSize, totalRecords, nextPageUrl);
            return Ok(response);
    }


    // GET: api/Categorias/5
    [HttpGet("{id:int:min(1)}")]
    public async Task<ActionResult<Category>> GetCategory(int id)
    {
        var categoria = await _repository.GetAsync(c => c.CategoryId == id);

        if (categoria == null)
        {
            return NotFound();
        }

        return categoria;
    }

    // PUT: api/Categorias/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id:int}")]
    public async Task<IActionResult> PutCategory(int id, Category category)
    {
            if (id != category.CategoryId)
            {
                return BadRequest();
            }

            if (!await _repository.ExistsAsync(c => c.CategoryId == id))
            {
                return NotFound();
            }

            await _repository.UpdateAsync(category);

            return NoContent();
    }

    // POST: api/Categorias
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Category>> PostCategory(Category category)
    {

        await _repository.CreateAsync(category);
        return CreatedAtAction("GetCategoria", new { id = category.CategoryId }, category);

    }

    // DELETE: api/Categorias/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        if (!await _repository.ExistsAsync(c => c.CategoryId == id))
        {
            return NotFound();
        }

        await _repository.DeleteAsync(c => c.CategoryId == id);

        return NoContent();

    }

}

