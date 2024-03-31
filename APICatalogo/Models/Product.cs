using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace APICatalogo.Models;

[Table("Products")]
public class Product
{
    [Key]
    public int ProductId { get; set; }
    [Required(ErrorMessage = "O nome é obrigatório")]
    [MaxLength(80)]
    public required string Name { get; set; }

    [Required(ErrorMessage = "A descricão é obrigatório")]
    [MaxLength(300)]
    public string? Description { get; set; }

    [Required(ErrorMessage = "O preço é obrigatório")]
    [Column(TypeName = "decimal(10, 2)")]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "A imagem URL é obrigatória")]
    [MaxLength(500)]
    public string? ImageUrl { get; set; }

    [Required(ErrorMessage = "O estoque é obrigatório")]
    [Range(0, 1000)]
    public float Stock { get; set; }

    [Required(ErrorMessage = "A data de cadastro é obrigatoria")]
    [DataType(DataType.DateTime)]
    public DateTime DataRegistration { get; set; } = DateTime.Now;

    [Required(ErrorMessage = "A data de alteração é obrigatoria")]
    [DataType(DataType.DateTime)]
    public DateTime DataUpdate { get; set; } = DateTime.Now;

    [Required(ErrorMessage = "A categoria é obrigatória")]
    public int CategoryId { get; set; }
    

    [JsonIgnore]
    public Category? Category { get; set; }
}
