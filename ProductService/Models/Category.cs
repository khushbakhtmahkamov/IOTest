﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProductService.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public ICollection<Product> Products { get; set; }
    }
}
