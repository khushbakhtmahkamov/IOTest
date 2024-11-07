using AutoMapper;
using ProductService.DTOs.Category;
using ProductService.Models;
using ProductService.Repositories.Category;
using ProductService.Utils;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductService.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repository;
        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CategoryDto>> GetAllAsync()
        {
            var categories = await _repository.GetAllAsync();
            var categoryDtos = _mapper.Map<IEnumerable<CategoryDto>>(categories);
            return categoryDtos;
        }

        public async Task<CategoryDto?> GetByIdAsync(int id)
        {
            var category = await GetCategoryByIdAsync(id);
            var categoryDto = _mapper.Map<CategoryDto>(category);
            return categoryDto;
        }

        public async Task<CategoryDto?> AddAsync(CreateUpdateCategoryDto createUpdateCategoryDto)
        {
            var category = _mapper.Map<Category>(createUpdateCategoryDto);
            category = await _repository.AddAsync(category);
            var categoryDto = _mapper.Map<CategoryDto>(category);
            return categoryDto;
        }

        public async Task<CategoryDto?> UpdateAsync(int id, CreateUpdateCategoryDto createUpdateCategoryDto)
        {
            var category = await GetCategoryByIdAsync(id);
            category.Name = createUpdateCategoryDto.Name;
            category = await _repository.UpdateAsync(category);
            var categoryDto = _mapper.Map<CategoryDto>(category);
            return categoryDto;
        }

        public async Task DeleteAsync(int id)
        {
            var category = await GetCategoryByIdAsync(id);
            await _repository.DeleteAsync(category);
        }

        private async Task<Category> GetCategoryByIdAsync(int id)
        {
            var category = await _repository.GetByIdAsync(id);
            if (category == null)
            {
                throw new BusinessException(BusinessExceptionCode.NOT_FOUND, "Category not found!");
            }
            return category;
        }

    }
}
