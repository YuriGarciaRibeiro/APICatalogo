using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace APICatalogo.DTOs
{
    public class ProductDto
    {
        public int ProductId { get; set; }
        [Required(ErrorMessage = "O nome é obrigatório")]
        [StringLength(80)]
        public required string Name { get; set; }

        [StringLength(255)]
        public string? Description { get; set; }

        [Required(ErrorMessage = "O preço é obrigatório")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "A imagem URL é obrigatória")]
        [StringLength(500)]
        public string? ImageUrl { get; set; }

        [Required(ErrorMessage = "A categoria é obrigatória")]
        public int CategoryId { get; set; }
    }
}