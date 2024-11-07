using AutoMapper;
using ProductService.DTOs.Category;
using ProductService.Models;

namespace ProductService.Mappings
{
    public class CategoryMapping : Profile
    {
        public CategoryMapping()
        {
            CreateMap<Category, CategoryDto>();
            CreateMap<CreateUpdateCategoryDto, Category>();
        }

    }
}
