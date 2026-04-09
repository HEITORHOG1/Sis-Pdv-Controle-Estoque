using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sis.Pdv.Blazor.Data.Entities;

/// <summary>
/// Entidade: Venda registrada no PDV local.
/// </summary>
[Table("Vendas")]
public class VendaEntity
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid ColaboradorId { get; set; }

    [Required, MaxLength(100)]
    public string NomeOperador { get; set; } = string.Empty;

    [Column(TypeName = "decimal(18,2)")]
    public decimal ValorTotal { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal ValorDesconto { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal ValorPago { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal Troco { get; set; }

    [Required, MaxLength(30)]
    public string FormaPagamento { get; set; } = string.Empty;

    [MaxLength(20)]
    public string? CpfCnpjCliente { get; set; }

    public DateTime DataVenda { get; set; } = DateTime.UtcNow;

    public bool Cancelada { get; set; }

    /// <summary>
    /// Indica se a venda já foi sincronizada com o banco principal.
    /// </summary>
    public bool Sincronizada { get; set; }

    public ICollection<ItemVendaEntity> Itens { get; set; } = [];
}
