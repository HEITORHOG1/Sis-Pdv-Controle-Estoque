using System.ComponentModel.DataAnnotations;

namespace Sis_Pdv_Controle_Estoque_Form.Dto.PDV
{
    /// <summary>
    /// DTO para seleção de lote em produtos perecíveis no PDV
    /// </summary>
    public class LoteSelecionadoDto
    {
        [Required(ErrorMessage = "Lote é obrigatório")]
        public string Lote { get; set; } = string.Empty;
        
        public DateTime? DataValidade { get; set; }
        
        [Range(0.01, double.MaxValue, ErrorMessage = "Quantidade disponível deve ser maior que zero")]
        public decimal QuantidadeDisponivel { get; set; }
        
        [Range(0.01, double.MaxValue, ErrorMessage = "Quantidade selecionada deve ser maior que zero")]
        public decimal QuantidadeSelecionada { get; set; }
        
        public bool EstaVencido => DataValidade.HasValue && DataValidade.Value < DateTime.Now;
        
        public bool VenceEm30Dias => DataValidade.HasValue && DataValidade.Value <= DateTime.Now.AddDays(30);
        
        public int DiasParaVencer => DataValidade.HasValue ? 
            Math.Max(0, (int)(DataValidade.Value - DateTime.Now).TotalDays) : int.MaxValue;
            
        /// <summary>
        /// Valida se a seleção de lote está correta
        /// </summary>
        public List<string> Validar()
        {
            var erros = new List<string>();
            
            if (string.IsNullOrWhiteSpace(Lote))
                erros.Add("Lote é obrigatório");
                
            if (QuantidadeSelecionada <= 0)
                erros.Add("Quantidade selecionada deve ser maior que zero");
                
            if (QuantidadeSelecionada > QuantidadeDisponivel)
                erros.Add($"Quantidade selecionada ({QuantidadeSelecionada}) maior que disponível ({QuantidadeDisponivel})");
                
            if (EstaVencido)
                erros.Add("Lote vencido não pode ser selecionado");
                
            return erros;
        }
        
        /// <summary>
        /// Obtém a cor para exibição baseada no vencimento
        /// </summary>
        public System.Drawing.Color ObterCorVencimento()
        {
            if (EstaVencido) return System.Drawing.Color.Red;
            if (VenceEm30Dias) return System.Drawing.Color.Orange;
            return System.Drawing.Color.Green;
        }
        
        /// <summary>
        /// Obtém o status do lote para exibição
        /// </summary>
        public string ObterStatusLote()
        {
            if (EstaVencido) return "VENCIDO";
            if (VenceEm30Dias) return $"VENCE EM {DiasParaVencer} DIAS";
            return "OK";
        }
    }
    
    /// <summary>
    /// DTO para resposta de lotes disponíveis
    /// </summary>
    public class LotesDisponiveisDto
    {
        public Guid ProdutoId { get; set; }
        public string CodigoBarras { get; set; } = string.Empty;
        public string NomeProduto { get; set; } = string.Empty;
        public decimal QuantidadeSolicitada { get; set; }
        public List<LoteSelecionadoDto> LotesDisponiveis { get; set; } = new();
        
        /// <summary>
        /// Obtém lotes válidos (não vencidos) ordenados por data de vencimento
        /// </summary>
        public List<LoteSelecionadoDto> ObterLotesValidos()
        {
            return LotesDisponiveis
                .Where(l => !l.EstaVencido)
                .OrderBy(l => l.DataValidade)
                .ToList();
        }
        
        /// <summary>
        /// Verifica se há lotes suficientes para atender a quantidade solicitada
        /// </summary>
        public bool TemQuantidadeSuficiente()
        {
            return ObterLotesValidos().Sum(l => l.QuantidadeDisponivel) >= QuantidadeSolicitada;
        }
        
        /// <summary>
        /// Sugere distribuição automática por FIFO (First In, First Out)
        /// </summary>
        public List<LoteSelecionadoDto> SugerirDistribuicaoFIFO()
        {
            var distribuicao = new List<LoteSelecionadoDto>();
            var quantidadeRestante = QuantidadeSolicitada;
            
            foreach (var lote in ObterLotesValidos())
            {
                if (quantidadeRestante <= 0) break;
                
                var quantidadeDoLote = Math.Min(quantidadeRestante, lote.QuantidadeDisponivel);
                
                distribuicao.Add(new LoteSelecionadoDto
                {
                    Lote = lote.Lote,
                    DataValidade = lote.DataValidade,
                    QuantidadeDisponivel = lote.QuantidadeDisponivel,
                    QuantidadeSelecionada = quantidadeDoLote
                });
                
                quantidadeRestante -= quantidadeDoLote;
            }
            
            return distribuicao;
        }
    }
}
