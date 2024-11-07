using System;
using System.ComponentModel.DataAnnotations;

namespace ProductService.DTOs.Product
{
    public class CreateUpdateProductDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        [Range(0.01, double.MaxValue)]
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
    }
}
