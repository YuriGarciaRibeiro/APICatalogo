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
using APICatalogo.Repositories.UnitOfWork;
using APICatalogo.DTOs;
using AutoMapper;
using APICatalogo.DTOs.CategoryDto;

namespace APICatalogo.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoriesController : ControllerBase
{

    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CategoriesController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }


    // GET: api/Categorias
    [HttpGet]
    public async Task<ActionResult<PaginatedResponse<CategoryResponseDto>>> GetCategories(int pageNumber = 1, int pageSize = 10)
    {

        var totalRecords = await _unitOfWork.CategoryRepository.CountAsync();
        var totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);

        // Validar e ajustar os parâmetros da página
        pageNumber = Math.Max(1, pageNumber);

        pageSize = Math.Max(1, Math.Min(pageSize, 100)); // Limita o tamanho máximo da página a 100

        var categorias = await _unitOfWork.CategoryRepository.GetAllAsync(pageNumber, pageSize);

        if (!categorias.Any())
        {
            return NotFound();
        }


        var categoriasDto = _mapper.Map<List<CategoryResponseDto>>(categorias);

        // Construir o URL para a próxima página (caso exista)
        string? nextPageUrl = null;
        if (pageNumber < totalPages)
        {
            nextPageUrl = Url.Action("GetCategorias", new { pageNumber = pageNumber + 1, pageSize = pageSize });
        }

        var response = new PaginatedResponse<CategoryResponseDto>(categoriasDto, pageNumber, pageSize, totalRecords, nextPageUrl);
        return Ok(response);
    }


    // GET: api/Categorias/5
    [HttpGet("{id:int:min(1)}")]
    public async Task<ActionResult<CategoryResponseDto>> GetCategory(int id)
    {
        var categoria = await _unitOfWork.CategoryRepository.GetAsync(c => c.CategoryId == id);

        if (categoria == null)
        {
            return NotFound();
        }

        var categoriaDto = _mapper.Map<CategoryResponseDto>(categoria);

        return categoriaDto;
    }

    // PUT: api/Categorias/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id:int}")]
    public async Task<IActionResult> PutCategory(int id, CategoryRequestDto category)
    {
        if (id != category.CategoryId)
        {
            return BadRequest();
        }

        if (!await _unitOfWork.CategoryRepository.ExistsAsync(c => c.CategoryId == id))
        {
            return NotFound();
        }

        var categoryToUpdate = _mapper.Map<Category>(category);

        _unitOfWork.CategoryRepository.Update(categoryToUpdate);
        await _unitOfWork.CommitAsync();

        return NoContent();
    }

    // POST: api/Categorias
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<CategoryResponseDto>> PostCategory(CategoryRequestDto category)
    {
        var categoryToAdd = _mapper.Map<Category>(category);

        _unitOfWork.CategoryRepository.CreateAsync(categoryToAdd);
        await _unitOfWork.CommitAsync();

        var categoryResponseDto = _mapper.Map<CategoryResponseDto>(categoryToAdd);

        return CreatedAtAction("PostCategory", new { id = categoryToAdd.CategoryId }, categoryResponseDto);
    }

    // DELETE: api/Categorias/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        if (!await _unitOfWork.CategoryRepository.ExistsAsync(c => c.CategoryId == id))
        {
            return NotFound();
        }

        await _unitOfWork.CategoryRepository.DeleteAsync(c => c.CategoryId == id);
        await _unitOfWork.CommitAsync();

        return NoContent();

    }

}

