using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace APICatalogo.DTOs
{
    public class CategoryDto
    {
        public int CategoryId { get; set; }
        [Required(ErrorMessage = "O nome é obrigatório")]
        [StringLength(80)]
        public required string Name { get; set; }

        [StringLength(255)]
        public string? ImageUrl { get; set; }

        
    }
}