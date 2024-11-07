using AutoMapper;
using Microsoft.Extensions.Logging;
using ProductService.DTOs;
using ProductService.DTOs.Product;
using ProductService.Models;
using ProductService.Repositories.Category;
using ProductService.Repositories.Product;
using ProductService.Utils;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductService.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ILogger<ProductService> _logger;
        private readonly IMapper _mapper;


        public ProductService(IProductRepository repository, ICategoryRepository categoryRepository, ILogger<ProductService> logger, IMapper mapper)
        {
            _repository = repository;
            _categoryRepository = categoryRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<PaginatedDto<ProductDto>> GetAllAsync(int pageNumber, int pageSize)
        {
            var totalProducts = await _repository.GetCount();
            var products = await _repository.GetProductsAsync(pageNumber, pageSize);
            var productDtos = _mapper.Map<IEnumerable<ProductDto>>(products);
            _logger.LogInformation("Get all product successfully!!");

            return new PaginatedDto<ProductDto>
            {
                TotalCount = totalProducts,
                PageSize = pageSize,
                PageNumber = pageNumber,
                Items = productDtos
            };
        }

        public async Task<ProductDto?> GetByIdAsync(int id)
        {
            var product = await GetProductByIdAsync(id);
            var categoryDto = _mapper.Map<ProductDto>(product);
            _logger.LogInformation("Get product byId {id} successfully!!", id);
            return categoryDto;
        }

        public async Task<ProductDto?> AddAsync(CreateUpdateProductDto createUpdateProductDto)
        {
            var category = await _categoryRepository.GetByIdAsync(createUpdateProductDto.CategoryId);
            if (category == null)
            {
                _logger.LogWarning("Category not found with id {id}", createUpdateProductDto.CategoryId);
                throw new BusinessException(BusinessExceptionCode.NOT_FOUND, "Category not found!");
            }

            var product = _mapper.Map<Product>(createUpdateProductDto);
            product.Category = category;
            product = await _repository.AddAsync(product);
            var productDto = _mapper.Map<ProductDto>(product);
            _logger.LogInformation("Create product successfully!!");
            return productDto;
        }

        public async Task<ProductDto?> UpdateAsync(int id, CreateUpdateProductDto createUpdateProductDto)
        {
            var category = await _categoryRepository.GetByIdAsync(createUpdateProductDto.CategoryId);
            if (category == null)
            {
                _logger.LogWarning("Category not found with id {id}", id);
                throw new BusinessException(BusinessExceptionCode.NOT_FOUND, "Category not found!");
            }
            var product = await GetProductByIdAsync(id);
            product.Name = createUpdateProductDto.Name; 
            product.Price = createUpdateProductDto.Price;
            product.Description = createUpdateProductDto.Description;
            product.Category = category;
            product = await _repository.UpdateAsync(product);
            var productDto = _mapper.Map<ProductDto>(product);
            _logger.LogInformation("update product successfully!!");
            return productDto;
        }

        public async Task DeleteAsync(int id)
        {
            var product = await GetProductByIdAsync(id);

            await _repository.DeleteAsync(product);
            _logger.LogInformation("Delete product successfully!!");
        }

        private async Task<Product> GetProductByIdAsync(int id)
        {
            var product = await _repository.GetByIdAsync(id);
            if (product == null)
            {
                _logger.LogWarning("Product not found with id {id}", id);
                throw new BusinessException(BusinessExceptionCode.NOT_FOUND, "Product not found!");
            }

            return product;
        }

    }
}
