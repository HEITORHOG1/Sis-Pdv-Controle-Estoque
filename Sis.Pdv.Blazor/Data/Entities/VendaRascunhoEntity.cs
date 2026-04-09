using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sis.Pdv.Blazor.Data.Entities;

/// <summary>
/// Rascunho de venda em andamento. Salvo automaticamente a cada alteracao
/// para permitir recuperacao em caso de queda de energia/crash.
/// Apenas UM rascunho ativo por caixa por vez.
/// </summary>
[Table("VendasRascunho")]
public class VendaRascunhoEntity
{
    [Key]
    public Guid Id { get; set; }

    public Guid ColaboradorId { get; set; }

    [Required, MaxLength(100)]
    public string NomeOperador { get; set; } = string.Empty;

    [MaxLength(20)]
    public string? CpfCnpjCliente { get; set; }

    public DateTime DataAbertura { get; set; }

    public DateTime UltimaAtualizacao { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// JSON serializado dos itens do carrinho (List of ItemCarrinhoDto)
    /// </summary>
    [Required]
    public string ItensJson { get; set; } = "[]";

    /// <summary>
    /// Numero do caixa para identificar de qual terminal veio
    /// </summary>
    [MaxLength(20)]
    public string NumeroCaixa { get; set; } = string.Empty;
}
