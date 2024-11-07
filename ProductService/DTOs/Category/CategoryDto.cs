using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace ProductService.DTOs.Category
{
    public class CategoryDto
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
    }
}
