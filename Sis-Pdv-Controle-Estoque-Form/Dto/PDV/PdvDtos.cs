using System.ComponentModel.DataAnnotations;

namespace Sis_Pdv_Controle_Estoque_Form.Dto.PDV
{
    /// <summary>
    /// DTO para item do carrinho de vendas
    /// </summary>
    public class ItemCarrinhoDto
    {
        public int Codigo { get; set; }
        
        [Required(ErrorMessage = "C�digo de barras � obrigat�rio")]
        public string CodigoBarras { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Descri��o do produto � obrigat�ria")]
        public string Descricao { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Pre�o unit�rio � obrigat�rio")]
        [Range(0.01, 999999.99, ErrorMessage = "Pre�o deve ser maior que zero")]
        public decimal PrecoUnitario { get; set; }
        
        [Required(ErrorMessage = "Quantidade � obrigat�ria")]
        [Range(1, 9999, ErrorMessage = "Quantidade deve ser entre 1 e 9999")]
        public int Quantidade { get; set; } = 1;
        
        public decimal Total => PrecoUnitario * Quantidade;
        public bool Cancelado { get; set; } = false;
        
        // Propriedades para controle de produto
        public Guid ProdutoId { get; set; }
        public int EstoqueDisponivel { get; set; }
        public DateTime? DataVencimento { get; set; }
        
        // Valida��es de neg�cio
        public List<string> Validar()
        {
            var erros = new List<string>();
            
            if (string.IsNullOrWhiteSpace(CodigoBarras))
                erros.Add("C�digo de barras � obrigat�rio");
            
            if (string.IsNullOrWhiteSpace(Descricao))
                erros.Add("Descri��o do produto � obrigat�ria");
            
            if (PrecoUnitario <= 0)
                erros.Add("Pre�o deve ser maior que zero");
            
            if (Quantidade <= 0)
                erros.Add("Quantidade deve ser maior que zero");
            
            if (Quantidade > EstoqueDisponivel)
                erros.Add($"Quantidade solicitada ({Quantidade}) � maior que o estoque dispon�vel ({EstoqueDisponivel})");
                
            // Verifica se produto est� pr�ximo ao vencimento
            if (DataVencimento.HasValue && DataVencimento.Value <= DateTime.Now.AddDays(7))
            {
                if (DataVencimento.Value <= DateTime.Now)
                    erros.Add("Produto vencido n�o pode ser vendido");
                else
                    erros.Add($"Produto vence em {(DataVencimento.Value - DateTime.Now).Days} dias");
            }
            
            return erros;
        }
        
        public string GetStatusDisplay()
        {
            if (Cancelado) return "CANCELADO";
            
            var alertas = new List<string>();
            
            if (DataVencimento.HasValue && DataVencimento.Value <= DateTime.Now.AddDays(7))
            {
                if (DataVencimento.Value <= DateTime.Now)
                    alertas.Add("VENCIDO");
                else
                    alertas.Add($"VENCE EM {(DataVencimento.Value - DateTime.Now).Days}D");
            }
            
            if (Quantidade > EstoqueDisponivel)
                alertas.Add("SEM ESTOQUE");
            
            return alertas.Any() ? string.Join(" | ", alertas) : "OK";
        }
    }
    
    /// <summary>
    /// DTO para controle de vendas no PDV
    /// </summary>
    public class VendaDto
    {
        public Guid Id { get; set; }
        public DateTime DataVenda { get; set; } = DateTime.Now;
        public string NomeOperador { get; set; } = string.Empty;
        public Guid ColaboradorId { get; set; }
        public string CpfCnpjCliente { get; set; } = string.Empty;
        public Guid? ClienteId { get; set; }
        public List<ItemCarrinhoDto> Itens { get; set; } = new();
        public decimal ValorTotal => Itens.Where(i => !i.Cancelado).Sum(i => i.Total);
        public decimal ValorDesconto { get; set; } = 0;
        public decimal ValorFinal => ValorTotal - ValorDesconto;
        public string FormaPagamento { get; set; } = string.Empty;
        public decimal ValorRecebido { get; set; }
        public decimal Troco => ValorRecebido - ValorFinal;
        public string StatusVenda { get; set; } = "ABERTA"; // ABERTA, FINALIZADA, CANCELADA
        
        public List<string> Validar()
        {
            var erros = new List<string>();
            
            if (!Itens.Any() || Itens.All(i => i.Cancelado))
                erros.Add("Venda deve ter pelo menos um item");
            
            if (ColaboradorId == Guid.Empty)
                erros.Add("Colaborador � obrigat�rio");
            
            if (string.IsNullOrWhiteSpace(FormaPagamento) && StatusVenda == "FINALIZADA")
                erros.Add("Forma de pagamento � obrigat�ria");
            
            if (ValorRecebido < ValorFinal && StatusVenda == "FINALIZADA")
                erros.Add("Valor recebido deve ser maior ou igual ao valor final");
            
            // Valida todos os itens
            foreach (var item in Itens.Where(i => !i.Cancelado))
            {
                var errosItem = item.Validar();
                erros.AddRange(errosItem.Select(e => $"Item {item.Codigo}: {e}"));
            }
            
            return erros;
        }
        
        public void AdicionarItem(ItemCarrinhoDto item)
        {
            // Verifica se j� existe item com mesmo c�digo de barras
            var itemExistente = Itens.FirstOrDefault(i => 
                i.CodigoBarras == item.CodigoBarras && !i.Cancelado);
            
            if (itemExistente != null)
            {
                itemExistente.Quantidade += item.Quantidade;
            }
            else
            {
                item.Codigo = Itens.Count + 1;
                Itens.Add(item);
            }
        }
        
        public void CancelarItem(int codigo)
        {
            var item = Itens.FirstOrDefault(i => i.Codigo == codigo);
            if (item != null)
            {
                item.Cancelado = true;
            }
        }
        
        public void RemoverItem(int codigo)
        {
            Itens.RemoveAll(i => i.Codigo == codigo);
            
            // Reordena c�digos
            for (int i = 0; i < Itens.Count; i++)
            {
                Itens[i].Codigo = i + 1;
            }
        }
        
        public int QuantidadeItensAtivos => Itens.Count(i => !i.Cancelado);
        public int QuantidadeItensCancelados => Itens.Count(i => i.Cancelado);
        
        public string GetResumoVenda()
        {
            return $"Itens: {QuantidadeItensAtivos} | " +
                   $"Total: {ValorTotal:C2} | " +
                   $"Cancelados: {QuantidadeItensCancelados}";
        }
    }
    
    /// <summary>
    /// DTO para configura��es do PDV
    /// </summary>
    public class ConfiguracaoPdvDto
    {
        public bool PermitirVendaSemEstoque { get; set; } = false;
        public bool AlertarProdutoVencimento { get; set; } = true;
        public int DiasAlertaVencimento { get; set; } = 7;
        public bool ImprimirCupomAutomatico { get; set; } = true;
        public bool SolicitarCpfCliente { get; set; } = false;
        public decimal DescontoMaximo { get; set; } = 100; // em %
        public bool PermitirAlterarQuantidade { get; set; } = true;
        public bool ExigirAutorizacaoCancelamento { get; set; } = true;
        public string ImpressoraFiscal { get; set; } = "BEMATECH MP-2100";
        public string NomeEmpresa { get; set; } = "HEITOR OLIVEIRA GON�ALVES - LTDA";
        public string EnderecoEmpresa { get; set; } = "Rua Professor jo�o de Deus n� 908 - Petr�polis-RJ";
        public string CnpjEmpresa { get; set; } = "71.564.173/0001-80";
        public string IeEmpresa { get; set; } = "714.145.789";
        public string ImEmpresa { get; set; } = "4567412";
    }
}