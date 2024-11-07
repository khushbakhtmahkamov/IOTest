using ProductService.DTOs.Category;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductService.Services
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDto>> GetAllAsync();
        Task<CategoryDto?> GetByIdAsync(int id);
        Task<CategoryDto?> AddAsync(CreateUpdateCategoryDto createUpdateCategoryDto);
        Task<CategoryDto?> UpdateAsync(int id, CreateUpdateCategoryDto createUpdateCategoryDto);
        Task DeleteAsync(int id);
    }
}
