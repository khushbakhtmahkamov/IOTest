using System.ComponentModel.DataAnnotations;

namespace ProductService.DTOs.Category
{
    public class CreateUpdateCategoryDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;
    }
}
