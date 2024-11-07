using AutoMapper;
using ProductService.DTOs.Product;
using ProductService.Models;

namespace ProductService.Mappings
{
    public class ProductMapping : Profile
    {
        public ProductMapping()
        {
            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.Category.Id));
            CreateMap<CreateUpdateProductDto, Product>();
        }
        
    }
}
