using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProductService.DTOs.Category;
using ProductService.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoryController : ControllerBase
    {

        private readonly ICategoryService _service;
        private readonly ILogger<CategoryController> _logger;

        public CategoryController(ILogger<CategoryController> logger, ICategoryService service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetCategorys()
        {
            var categoryDTOs = await _service.GetAllAsync();
            return Ok(categoryDTOs);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryDto>> GetCategory(int id)
        {
            var categoryDTO = await _service.GetByIdAsync(id);
            return Ok(categoryDTO);
        }

        [HttpPost]
        public async Task<ActionResult<CategoryDto>> CreateCategory([FromBody] CreateUpdateCategoryDto createUpdateCategoryDto)
        {
            var categoryDTO = await _service.AddAsync(createUpdateCategoryDto);
            return Ok(categoryDTO);

        }

        [HttpPut("{id}")]
        public async Task<ActionResult<CategoryDto>> UpdateCategory(int id, [FromBody] CreateUpdateCategoryDto createUpdateCategoryDto)
        {
            var categoryDTO = await _service.UpdateAsync(id, createUpdateCategoryDto);
            return Ok(categoryDTO);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
