using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProductService.DTOs;
using ProductService.DTOs.Product;
using ProductService.Services;
using System.Threading.Tasks;

namespace ProductService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {

        private readonly IProductService _service;
        private readonly ILogger<ProductController> _logger;
        private readonly IMapper _mapper;

        public ProductController(ILogger<ProductController> logger, IProductService service, IMapper mapper)
        {
            _logger = logger;
            _service = service;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<PaginatedDto<ProductDto>>> GetProducts(int pageNumber = 1, int pageSize = 10)
        {
            var productDtos = await _service.GetAllAsync(pageNumber,pageSize);
            return Ok(productDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetProduct(int id)
        {
            var productDto = await _service.GetByIdAsync(id);
            return Ok(productDto);
        }

        [HttpPost]
        public async Task<ActionResult<ProductDto>> CreateProduct([FromBody] CreateUpdateProductDto createUpdateProductDto)
        {
            var productDto = await _service.AddAsync(createUpdateProductDto);
            return Ok(productDto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ProductDto>> UpdateProduct(int id, [FromBody] CreateUpdateProductDto createUpdateProductDto)
        {
            var productDto = await _service.UpdateAsync(id,createUpdateProductDto);
            return Ok(productDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
