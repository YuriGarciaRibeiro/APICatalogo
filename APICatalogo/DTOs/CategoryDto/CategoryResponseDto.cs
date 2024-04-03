using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APICatalogo.DTOs.CategoryDto
{
    public class CategoryResponseDto
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
    }
}