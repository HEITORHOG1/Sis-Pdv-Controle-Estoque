using System.ComponentModel.DataAnnotations;

namespace Sis_Pdv_Controle_Estoque_Form.Dto.Movimentacao
{
    /// <summary>
    /// DTO para criação de movimentações de estoque
    /// </summary>
    public class CriarMovimentacaoDto
    {
        [Required(ErrorMessage = "O ID do produto é obrigatório")]
        public Guid ProdutoId { get; set; }

        [Required(ErrorMessage = "A quantidade é obrigatória")]
        [Range(0.01, double.MaxValue, ErrorMessage = "A quantidade deve ser maior que zero")]
        public decimal Quantidade { get; set; }

        [Required(ErrorMessage = "O tipo de movimentação é obrigatório")]
        [Range(1, 4, ErrorMessage = "Tipo de movimentação inválido")]
        public int Tipo { get; set; } // 1=Entrada, 2=Saída, 3=Ajuste, 4=Transferência

        [Required(ErrorMessage = "O motivo da movimentação é obrigatório")]
        [StringLength(500, ErrorMessage = "O motivo deve ter no máximo 500 caracteres")]
        public string Motivo { get; set; } = string.Empty;

        [Range(0.01, double.MaxValue, ErrorMessage = "O custo unitário deve ser maior que zero")]
        public decimal? CustoUnitario { get; set; }

        [StringLength(50, ErrorMessage = "O lote deve ter no máximo 50 caracteres")]
        public string? Lote { get; set; }

        public DateTime? DataValidade { get; set; }

        [StringLength(100, ErrorMessage = "A referência deve ter no máximo 100 caracteres")]
        public string? Referencia { get; set; }

        /// <summary>
        /// Valida se é uma movimentação de produto perecível
        /// </summary>
        public bool ValidarProdutoPerecivel(bool isPerecivel)
        {
            if (isPerecivel && (Tipo == 1 || Tipo == 3)) // Entrada ou Ajuste
            {
                return !string.IsNullOrWhiteSpace(Lote) && DataValidade.HasValue;
            }
            return true;
        }

        /// <summary>
        /// Obtém a descrição do tipo de movimentação
        /// </summary>
        public string ObterDescricaoTipo()
        {
            return Tipo switch
            {
                1 => "Entrada",
                2 => "Saída",
                3 => "Ajuste",
                4 => "Transferência",
                _ => "Desconhecido"
            };
        }
    }

    /// <summary>
    /// DTO para resposta de movimentação de estoque
    /// </summary>
    public class MovimentacaoEstoqueDto
    {
        public Guid Id { get; set; }
        public Guid ProdutoId { get; set; }
        public string ProdutoNome { get; set; } = string.Empty;
        public string CodigoBarras { get; set; } = string.Empty;
        public decimal Quantidade { get; set; }
        public int Tipo { get; set; }
        public string TipoDescricao { get; set; } = string.Empty;
        public string Motivo { get; set; } = string.Empty;
        public decimal? CustoUnitario { get; set; }
        public decimal EstoqueAnterior { get; set; }
        public decimal EstoqueNovo { get; set; }
        public string? Lote { get; set; }
        public DateTime? DataValidade { get; set; }
        public string? Referencia { get; set; }
        public DateTime DataMovimentacao { get; set; }
        public Guid? UsuarioId { get; set; }
        public string? UsuarioNome { get; set; }

        /// <summary>
        /// Obtém a cor para exibição baseada no tipo de movimentação
        /// </summary>
        public System.Drawing.Color ObterCorTipo()
        {
            return Tipo switch
            {
                1 => System.Drawing.Color.Green,      // Entrada
                2 => System.Drawing.Color.Red,        // Saída
                3 => System.Drawing.Color.Orange,     // Ajuste
                4 => System.Drawing.Color.Blue,       // Transferência
                _ => System.Drawing.Color.Black
            };
        }
    }

    /// <summary>
    /// DTO para filtros de movimentação
    /// </summary>
    public class FiltroMovimentacaoDto
    {
        public int Pagina { get; set; } = 1;
        public int TamanhoPagina { get; set; } = 20;
        public Guid? ProdutoId { get; set; }
        public int? Tipo { get; set; }
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
        public string? Lote { get; set; }
        public string? Referencia { get; set; }
        public string? NomeProduto { get; set; }
        public string? CodigoBarras { get; set; }

        /// <summary>
        /// Valida os filtros aplicados
        /// </summary>
        public bool ValidarFiltros()
        {
            if (DataInicio.HasValue && DataFim.HasValue && DataInicio > DataFim)
                return false;

            if (Pagina <= 0 || TamanhoPagina <= 0)
                return false;

            return true;
        }
    }

    /// <summary>
    /// DTO para alertas de estoque
    /// </summary>
    public class AlertaEstoqueDto
    {
        public Guid ProdutoId { get; set; }
        public string ProdutoNome { get; set; } = string.Empty;
        public string CodigoBarras { get; set; } = string.Empty;
        public decimal EstoqueAtual { get; set; }
        public decimal PontoReposicao { get; set; }
        public decimal EstoqueMinimo { get; set; }
        public string TipoAlerta { get; set; } = string.Empty;
        public string Mensagem { get; set; } = string.Empty;
        public DateTime DataAlerta { get; set; }
        public List<LoteVencimentoDto>? LotesVencendo { get; set; }

        /// <summary>
        /// Obtém a prioridade do alerta (1=Alto, 2=Médio, 3=Baixo)
        /// </summary>
        public int ObterPrioridade()
        {
            return TipoAlerta.ToUpper() switch
            {
                "OUTOFSTOCK" => 1,
                "EXPIREDBATCH" => 1,
                "LOWSTOCK" => 2,
                "EXPIRINGBATCH" => 3,
                _ => 3
            };
        }

        /// <summary>
        /// Obtém a cor do alerta
        /// </summary>
        public System.Drawing.Color ObterCorAlerta()
        {
            return ObterPrioridade() switch
            {
                1 => System.Drawing.Color.Red,
                2 => System.Drawing.Color.Orange,
                3 => System.Drawing.Color.Yellow,
                _ => System.Drawing.Color.Gray
            };
        }
    }

    /// <summary>
    /// DTO para informações de lote e vencimento
    /// </summary>
    public class LoteVencimentoDto
    {
        public string Lote { get; set; } = string.Empty;
        public DateTime? DataValidade { get; set; }
        public decimal QuantidadeDisponivel { get; set; }
        public bool EstaVencido => DataValidade.HasValue && DataValidade.Value < DateTime.Now;
        public bool VenceEm30Dias => DataValidade.HasValue && DataValidade.Value <= DateTime.Now.AddDays(30);
        public int DiasParaVencer => DataValidade.HasValue ? 
            Math.Max(0, (int)(DataValidade.Value - DateTime.Now).TotalDays) : int.MaxValue;

        /// <summary>
        /// Obtém a cor baseada no vencimento
        /// </summary>
        public System.Drawing.Color ObterCorVencimento()
        {
            if (EstaVencido) return System.Drawing.Color.Red;
            if (VenceEm30Dias) return System.Drawing.Color.Orange;
            return System.Drawing.Color.Green;
        }
    }

    /// <summary>
    /// DTO para validação de estoque
    /// </summary>
    public class ValidacaoEstoqueDto
    {
        public Guid ProdutoId { get; set; }
        public decimal QuantidadeSolicitada { get; set; }
        public string? Lote { get; set; }
        public bool EstaDisponivel { get; set; }
        public decimal QuantidadeDisponivel { get; set; }
        public decimal QuantidadeFaltante => Math.Max(0, QuantidadeSolicitada - QuantidadeDisponivel);
        public string Mensagem { get; set; } = string.Empty;
        public List<LoteVencimentoDto>? LotesDisponiveis { get; set; }
    }

    /// <summary>
    /// DTO para resposta paginada de movimentações
    /// </summary>
    public class MovimentacoesPaginadasDto
    {
        public List<MovimentacaoEstoqueDto> Itens { get; set; } = new();
        public int TotalRegistros { get; set; }
        public int NumeroPagina { get; set; }
        public int TamanhoPagina { get; set; }
        public int TotalPaginas => (int)Math.Ceiling((double)TotalRegistros / TamanhoPagina);
        public bool TemPaginaAnterior => NumeroPagina > 1;
        public bool TemProximaPagina => NumeroPagina < TotalPaginas;
    }

    /// <summary>
    /// Enum para tipos de movimentação
    /// </summary>
    public enum TipoMovimentacao
    {
        Entrada = 1,
        Saida = 2,
        Ajuste = 3,
        Transferencia = 4
    }

    /// <summary>
    /// Enum para tipos de alerta
    /// </summary>
    public enum TipoAlerta
    {
        SemEstoque = 1,
        EstoqueBaixo = 2,
        LoteVencendo = 3,
        LoteVencido = 4
    }
}
