using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sis.Pdv.Blazor.Data.Entities;

/// <summary>
/// Entidade: Item de uma venda no PDV local.
/// </summary>
[Table("ItensVenda")]
public class ItemVendaEntity
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid VendaId { get; set; }

    public Guid ProdutoId { get; set; }

    public int Sequencial { get; set; }

    [Required, MaxLength(50)]
    public string CodigoBarras { get; set; } = string.Empty;

    [Required, MaxLength(200)]
    public string Descricao { get; set; } = string.Empty;

    [Column(TypeName = "decimal(18,2)")]
    public decimal PrecoUnitario { get; set; }

    public int Quantidade { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal Total { get; set; }

    public bool Cancelado { get; set; }

    [ForeignKey(nameof(VendaId))]
    public VendaEntity Venda { get; set; } = null!;
}
