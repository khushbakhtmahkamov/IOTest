using ProductService.DTOs;
using ProductService.DTOs.Product;
using ProductService.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductService.Services
{
    public interface IProductService
    {
        Task<PaginatedDto<ProductDto>> GetAllAsync(int pageNumber, int pageSize);
        Task<ProductDto?> GetByIdAsync(int id);
        Task<ProductDto?> AddAsync(CreateUpdateProductDto createUpdateProductDto);
        Task<ProductDto?> UpdateAsync(int id, CreateUpdateProductDto createUpdateProductDto);
        Task DeleteAsync(int id);
    }
}
