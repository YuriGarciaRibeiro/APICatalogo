using APICatalogo.Validation;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace APICatalogo.Models;

[Table("Categories")]
public class Category
{   
    public Category()
    {
        Products = new Collection<Product>();
    }

    [Key]
    public int CategoryId { get; set; }

    [Required(ErrorMessage = "O nome é obrigatorio")]
    [MaxLength(80)]
    [FirstCapitalLetter]
    public string Name { get; set; }

    [Required(ErrorMessage = "A descricão é obrigatorio")]
    [MaxLength(300)]
    public string? ImageUrl { get; set; }

    [Required(ErrorMessage = "A data de cadastro é obrigatoria")]
    [DataType(DataType.DateTime)]
    public DateTime DataRegistration { get; set; } = DateTime.Now;

    [Required(ErrorMessage = "A data de alteração é obrigatoria")]
    [DataType(DataType.DateTime)]
    public DateTime DataUpdate { get; set; } = DateTime.Now;
    
    [Required]
    [JsonIgnore]
    public ICollection<Product>? Products { get; set; }
}

