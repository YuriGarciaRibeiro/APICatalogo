using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace APICatalogo.Models;

[Table("Produtos")]
public class Produto
{
    [Key]
    public int ProdutoId { get; set; }
    [Required]
    [MaxLength(80)]
    public string? Nome { get; set; }

    [Required]
    [MaxLength(300)]
    public string? Descricao { get; set; }

    [Required]
    [Column(TypeName = "decimal(10, 2)")]
    public decimal Preco { get; set; }
    [Required]
    [MaxLength(500)]
    public string? ImagemUrl { get; set; }
    [Required]
    [Range(0, 1000)]
    public float Estoque { get; set; }
    [Required]
    [DataType(DataType.DateTime)]
    public DateTime DataCadastro { get; set; }

    [Required]
    public int CategoriaId { get; set; }

    [Required]
    [JsonIgnore]
    public required Categoria Categoria { get; set; }
}
