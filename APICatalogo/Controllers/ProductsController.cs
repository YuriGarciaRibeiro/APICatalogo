using Microsoft.AspNetCore.Mvc;
using APICatalogo.Models;
using APICatalogo.Responses;
using APICatalogo.Repositories.UnitOfWork;
using APICatalogo.DTOs;
using AutoMapper;

namespace APICatalogo.Controllers;

    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }



        // GET: api/Produtos
        [HttpGet]
        public async Task<ActionResult<PaginatedResponse<ProductDto>>> GetProducts(int pageNumber = 1, int pageSize = 10)
        {
            
                var totalRecords = await _unitOfWork.ProductRepository.CountAsync();
                var totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);

                // Validar e ajustar os parâmetros da página
                pageNumber = Math.Max(1, pageNumber);
                pageSize = Math.Max(1, Math.Min(pageSize, 100)); // Limita o tamanho máximo da página a 100

                var Products = await _unitOfWork.ProductRepository.GetAllAsync(pageNumber, pageSize);

                var ProductsDtos = _mapper.Map<IEnumerable<ProductDto>>(Products);

                // Construir o URL para a próxima página
                string? nextPageUrl = null;
                if (pageNumber < totalPages)
                {
                    nextPageUrl = Url.Action("GetProdutos", new { pageNumber = pageNumber + 1, pageSize = pageSize });
                }

                var response = new PaginatedResponse<ProductDto>(ProductsDtos, pageNumber, pageSize, totalRecords, nextPageUrl);
                return Ok(response);
        }


        // GET: api/Produtoes/5
        [HttpGet("{id:int:min(1)}")]
        public async Task<ActionResult<ProductDto>> GetProduct(int id)
        {
        
            var product = await _unitOfWork.ProductRepository.GetAsync(c => c.ProductId == id);

            if (product == null)
            {
                return NotFound();
            }

            return _mapper.Map<ProductDto>(product);

        }

        // PUT: api/Produtoes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id:int:min(1)}")]
        public async Task<IActionResult> PutProduct(int id, [FromBody] Product product)
        {

            if (id != product.ProductId)
            {
                return BadRequest();
            }

            if (!await _unitOfWork.ProductRepository.ExistsAsync(c => c.ProductId == id))
            {
                return NotFound();
            }


            _unitOfWork.ProductRepository.Update(product);
            await _unitOfWork.CommitAsync();
            
            return NoContent();
        }

        // POST: api/Produtoes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ProductDto>> PostProduct(ProductDto productDto)
        {
            var product = _mapper.Map<Product>(productDto);

            _unitOfWork.ProductRepository.CreateAsync(product);
            await _unitOfWork.CommitAsync();
            

            return CreatedAtAction("PostProduct", new { id = product.ProductId }, productDto);

        }

        // DELETE: api/Produtoes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {   
            if (!await _unitOfWork.ProductRepository.ExistsAsync(c => c.ProductId == id))
            {
                return NotFound();
            }
           
            await _unitOfWork.ProductRepository.DeleteAsync(c => c.ProductId == id);
            await _unitOfWork.CommitAsync();

            return NoContent();

        }

        

    }