using System.ComponentModel.DataAnnotations;

namespace Sis_Pdv_Controle_Estoque_Form.Dto.Produto
{
    public class ProdutoDto
    {
        public Guid Id { get; set; }
        
        [Required(ErrorMessage = "C�digo de barras � obrigat�rio")]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "C�digo de barras deve ter entre 8 e 20 caracteres")]
        public string CodBarras { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Nome do produto � obrigat�rio")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Nome deve ter entre 2 e 100 caracteres")]
        public string NomeProduto { get; set; } = string.Empty;
        
        [StringLength(500, ErrorMessage = "Descri��o n�o pode ter mais de 500 caracteres")]
        public string DescricaoProduto { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Pre�o de custo � obrigat�rio")]
        [Range(0.01, 999999.99, ErrorMessage = "Pre�o de custo deve ser entre R$ 0,01 e R$ 999.999,99")]
        public decimal PrecoCusto { get; set; }
        
        [Required(ErrorMessage = "Pre�o de venda � obrigat�rio")]
        [Range(0.01, 999999.99, ErrorMessage = "Pre�o de venda deve ser entre R$ 0,01 e R$ 999.999,99")]
        public decimal PrecoVenda { get; set; }
        
        [Range(0, 9999.99, ErrorMessage = "Margem de lucro deve ser entre 0% e 9999%")]
        public decimal MargemLucro { get; set; }
        
        public DateTime DataFabricao { get; set; }
        public DateTime DataVencimento { get; set; }
        
        [Required(ErrorMessage = "Quantidade em estoque � obrigat�ria")]
        [Range(0, int.MaxValue, ErrorMessage = "Quantidade deve ser maior ou igual a zero")]
        public int QuantidadeEstoqueProduto { get; set; }
        
        [Required(ErrorMessage = "Fornecedor � obrigat�rio")]
        public Guid FornecedorId { get; set; }
        
        [Required(ErrorMessage = "Categoria � obrigat�ria")]
        public Guid CategoriaId { get; set; }
        
        [Range(0, 1, ErrorMessage = "Status deve ser 0 (inativo) ou 1 (ativo)")]
        public int StatusAtivo { get; set; } = 1;

        // Propriedades adicionais para auditoria
        public DateTime? DataCriacao { get; set; }
        public DateTime? DataAtualizacao { get; set; }

        // Propriedades calculadas
        public decimal ValorTotalEstoque => PrecoCusto * QuantidadeEstoqueProduto;
        public bool EhPerecivel => DataVencimento > DateTime.MinValue && DataVencimento > DataFabricao;
        public int DiasVencimento => EhPerecivel ? (DataVencimento - DateTime.Now).Days : 0;
        public bool EstoqueMinimo => QuantidadeEstoqueProduto <= 10; // Considerar estoque m�nimo como 10

        // M�todo para validar o DTO
        public List<string> Validar()
        {
            var erros = new List<string>();

            // Valida��o C�digo de Barras
            if (string.IsNullOrWhiteSpace(CodBarras))
            {
                erros.Add("C�digo de barras � obrigat�rio");
            }
            else if (CodBarras.Trim().Length < 8)
            {
                erros.Add("C�digo de barras deve ter pelo menos 8 caracteres");
            }
            else if (CodBarras.Length > 20)
            {
                erros.Add("C�digo de barras n�o pode ter mais de 20 caracteres");
            }
            else if (!System.Text.RegularExpressions.Regex.IsMatch(CodBarras, @"^[0-9]+$"))
            {
                erros.Add("C�digo de barras deve conter apenas n�meros");
            }

            // Valida��o Nome
            if (string.IsNullOrWhiteSpace(NomeProduto))
            {
                erros.Add("Nome do produto � obrigat�rio");
            }
            else if (NomeProduto.Trim().Length < 2)
            {
                erros.Add("Nome deve ter pelo menos 2 caracteres");
            }
            else if (NomeProduto.Length > 100)
            {
                erros.Add("Nome n�o pode ter mais de 100 caracteres");
            }

            // Valida��o Descri��o
            if (!string.IsNullOrEmpty(DescricaoProduto) && DescricaoProduto.Length > 500)
            {
                erros.Add("Descri��o n�o pode ter mais de 500 caracteres");
            }

            // Valida��o Pre�os
            if (PrecoCusto <= 0)
            {
                erros.Add("Pre�o de custo deve ser maior que zero");
            }
            else if (PrecoCusto > 999999.99m)
            {
                erros.Add("Pre�o de custo n�o pode ser maior que R$ 999.999,99");
            }

            if (PrecoVenda <= 0)
            {
                erros.Add("Pre�o de venda deve ser maior que zero");
            }
            else if (PrecoVenda > 999999.99m)
            {
                erros.Add("Pre�o de venda n�o pode ser maior que R$ 999.999,99");
            }

            // Valida��o Margem de Lucro
            if (MargemLucro < 0)
            {
                erros.Add("Margem de lucro n�o pode ser negativa");
            }
            else if (MargemLucro > 9999.99m)
            {
                erros.Add("Margem de lucro n�o pode ser maior que 9999%");
            }

            // Valida��o de l�gica de neg�cio: pre�o de venda deve ser maior que custo
            if (PrecoCusto > 0 && PrecoVenda > 0 && PrecoVenda <= PrecoCusto)
            {
                erros.Add("Pre�o de venda deve ser maior que o pre�o de custo");
            }

            // Valida��o Datas para produtos perec�veis
            if (DataVencimento > DateTime.MinValue && DataFabricao > DateTime.MinValue)
            {
                if (DataVencimento <= DataFabricao)
                {
                    erros.Add("Data de vencimento deve ser posterior � data de fabrica��o");
                }

                if (DataFabricao > DateTime.Now.AddDays(1))
                {
                    erros.Add("Data de fabrica��o n�o pode ser futura");
                }

                if (DataVencimento <= DateTime.Now)
                {
                    erros.Add("Produto com data de vencimento expirada n�o pode ser cadastrado");
                }
            }

            // Valida��o Quantidade
            if (QuantidadeEstoqueProduto < 0)
            {
                erros.Add("Quantidade em estoque n�o pode ser negativa");
            }

            // Valida��o Fornecedor
            if (FornecedorId == Guid.Empty)
            {
                erros.Add("Fornecedor � obrigat�rio");
            }

            // Valida��o Categoria
            if (CategoriaId == Guid.Empty)
            {
                erros.Add("Categoria � obrigat�ria");
            }

            // Valida��o do ID quando presente
            if (Id != Guid.Empty && Id == Guid.Empty)
            {
                erros.Add("ID deve ser um GUID v�lido");
            }

            return erros;
        }

        // M�todo para normalizar dados
        public void NormalizarDados()
        {
            if (!string.IsNullOrEmpty(NomeProduto))
            {
                NomeProduto = System.Text.RegularExpressions.Regex.Replace(
                    NomeProduto.Trim(), @"\s+", " ");
                var textInfo = System.Globalization.CultureInfo.CurrentCulture.TextInfo;
                NomeProduto = textInfo.ToTitleCase(NomeProduto.ToLower());
            }

            if (!string.IsNullOrEmpty(DescricaoProduto))
            {
                DescricaoProduto = DescricaoProduto.Trim();
            }

            if (!string.IsNullOrEmpty(CodBarras))
            {
                CodBarras = CodBarras.Trim().Replace(" ", "").Replace("-", "");
            }

            // Recalcula margem de lucro se necess�rio
            if (PrecoCusto > 0 && PrecoVenda > 0)
            {
                MargemLucro = ((PrecoVenda / PrecoCusto) - 1) * 100;
                MargemLucro = Math.Round(MargemLucro, 2);
            }
        }

        // M�todo para calcular margem de lucro
        public decimal CalcularMargemLucro()
        {
            if (PrecoCusto <= 0) return 0;
            return Math.Round(((PrecoVenda / PrecoCusto) - 1) * 100, 2);
        }

        // M�todo para calcular pre�o de venda baseado na margem
        public decimal CalcularPrecoVendaPorMargem(decimal margemDesejada)
        {
            if (PrecoCusto <= 0) return 0;
            return Math.Round(PrecoCusto * (1 + margemDesejada / 100), 2);
        }

        // M�todo para verificar se produto est� com estoque baixo
        public bool EstoqueAbaixoMinimo(int estoqueMinimo = 10)
        {
            return QuantidadeEstoqueProduto <= estoqueMinimo;
        }

        // M�todo para verificar se produto est� pr�ximo do vencimento
        public bool ProximoVencimento(int diasAlerta = 30)
        {
            if (!EhPerecivel) return false;
            return DiasVencimento <= diasAlerta && DiasVencimento > 0;
        }

        // Override ToString para melhor exibi��o
        public override string ToString()
        {
            return $"{NomeProduto} - {CodBarras}";
        }

        // M�todo para verificar se � v�lido
        public bool EhValido()
        {
            return Validar().Count == 0;
        }

        // M�todo para obter status formatado
        public string GetStatusFormatado()
        {
            return StatusAtivo == 1 ? "Ativo" : "Inativo";
        }

        // M�todo para obter informa��es de estoque
        public string GetInfoEstoque()
        {
            var info = $"Qtd: {QuantidadeEstoqueProduto}";
            
            if (EstoqueMinimo)
                info += " (BAIXO)";
                
            if (EhPerecivel && ProximoVencimento())
                info += $" - Vence em {DiasVencimento} dias";
                
            return info;
        }
    }
}
