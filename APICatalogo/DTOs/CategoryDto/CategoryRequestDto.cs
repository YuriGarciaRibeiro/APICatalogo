using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace APICatalogo.DTOs.CategoryDto
{
    public class CategoryRequestDto
    {

        public int? CategoryId { get; set; }
        [Required(ErrorMessage = "O nome é obrigatório")]
        [StringLength(80)]
        public string Name { get; set; }
        [StringLength(255)]
        [Url(ErrorMessage = "URL inválida")]
        [Required(ErrorMessage = "A URL da imagem é obrigatória")]
        public string ImageUrl { get; set; }
    }
}