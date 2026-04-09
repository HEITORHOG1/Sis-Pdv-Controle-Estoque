using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sis.Pdv.Blazor.Data.Entities;

/// <summary>
/// Entidade: Produto no banco local do PDV.
/// Sincronizado via RabbitMQ a partir do banco principal.
/// </summary>
[Table("Produtos")]
public class ProdutoEntity
{
    [Key]
    public Guid Id { get; set; }

    [Required, MaxLength(200)]
    public string NomeProduto { get; set; } = string.Empty;

    [Required, MaxLength(50)]
    public string CodBarras { get; set; } = string.Empty;

    [Column(TypeName = "decimal(18,2)")]
    public decimal PrecoVenda { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal PrecoCusto { get; set; }

    public int QuantidadeEstoqueProduto { get; set; }

    [MaxLength(500)]
    public string? Descricao { get; set; }

    public Guid? CategoriaId { get; set; }

    public Guid? FornecedorId { get; set; }

    public DateTime? DataVencimento { get; set; }

    public DateTime DataAtualizacao { get; set; } = DateTime.UtcNow;
}
