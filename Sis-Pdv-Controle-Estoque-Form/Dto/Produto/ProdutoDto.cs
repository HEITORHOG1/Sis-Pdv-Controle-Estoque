using System.ComponentModel.DataAnnotations;

namespace Sis_Pdv_Controle_Estoque_Form.Dto.Produto
{
    public class ProdutoDto
    {
        public Guid Id { get; set; }
        
        [Required(ErrorMessage = "Código de barras é obrigatório")]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "Código de barras deve ter entre 8 e 20 caracteres")]
        public string codBarras { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Nome do produto é obrigatório")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Nome deve ter entre 2 e 100 caracteres")]
        public string nomeProduto { get; set; } = string.Empty;
        
        [StringLength(500, ErrorMessage = "Descrição não pode ter mais de 500 caracteres")]
        public string descricaoProduto { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Preço de custo é obrigatório")]
        [Range(0.01, 999999.99, ErrorMessage = "Preço de custo deve ser entre R$ 0,01 e R$ 999.999,99")]
        public decimal precoCusto { get; set; }
        
        [Required(ErrorMessage = "Preço de venda é obrigatório")]
        [Range(0.01, 999999.99, ErrorMessage = "Preço de venda deve ser entre R$ 0,01 e R$ 999.999,99")]
        public decimal precoVenda { get; set; }
        
        [Range(0, 9999.99, ErrorMessage = "Margem de lucro deve ser entre 0% e 9999%")]
        public decimal margemLucro { get; set; }
        
        public DateTime dataFabricao { get; set; }
        public DateTime dataVencimento { get; set; }
        
        [Required(ErrorMessage = "Quantidade em estoque é obrigatória")]
        [Range(0, int.MaxValue, ErrorMessage = "Quantidade deve ser maior ou igual a zero")]
        public int quatidadeEstoqueProduto { get; set; }
        
        [Required(ErrorMessage = "Fornecedor é obrigatório")]
        public Guid FornecedorId { get; set; }
        
        [Required(ErrorMessage = "Categoria é obrigatória")]
        public Guid CategoriaId { get; set; }
        
        [Range(0, 1, ErrorMessage = "Status deve ser 0 (inativo) ou 1 (ativo)")]
        public int statusAtivo { get; set; } = 1;

        // Propriedades adicionais para auditoria
        public DateTime? DataCriacao { get; set; }
        public DateTime? DataAtualizacao { get; set; }

        // Propriedades calculadas
        public decimal ValorTotalEstoque => precoCusto * quatidadeEstoqueProduto;
        public bool EhPerecivel => dataVencimento > DateTime.MinValue && dataVencimento > dataFabricao;
        public int DiasVencimento => EhPerecivel ? (dataVencimento - DateTime.Now).Days : 0;
        public bool EstoqueMinimo => quatidadeEstoqueProduto <= 10; // Considerar estoque mínimo como 10

        // Método para validar o DTO
        public List<string> Validar()
        {
            var erros = new List<string>();

            // Validação Código de Barras
            if (string.IsNullOrWhiteSpace(codBarras))
            {
                erros.Add("Código de barras é obrigatório");
            }
            else if (codBarras.Trim().Length < 8)
            {
                erros.Add("Código de barras deve ter pelo menos 8 caracteres");
            }
            else if (codBarras.Length > 20)
            {
                erros.Add("Código de barras não pode ter mais de 20 caracteres");
            }
            else if (!System.Text.RegularExpressions.Regex.IsMatch(codBarras, @"^[0-9]+$"))
            {
                erros.Add("Código de barras deve conter apenas números");
            }

            // Validação Nome
            if (string.IsNullOrWhiteSpace(nomeProduto))
            {
                erros.Add("Nome do produto é obrigatório");
            }
            else if (nomeProduto.Trim().Length < 2)
            {
                erros.Add("Nome deve ter pelo menos 2 caracteres");
            }
            else if (nomeProduto.Length > 100)
            {
                erros.Add("Nome não pode ter mais de 100 caracteres");
            }

            // Validação Descrição
            if (!string.IsNullOrEmpty(descricaoProduto) && descricaoProduto.Length > 500)
            {
                erros.Add("Descrição não pode ter mais de 500 caracteres");
            }

            // Validação Preços
            if (precoCusto <= 0)
            {
                erros.Add("Preço de custo deve ser maior que zero");
            }
            else if (precoCusto > 999999.99m)
            {
                erros.Add("Preço de custo não pode ser maior que R$ 999.999,99");
            }

            if (precoVenda <= 0)
            {
                erros.Add("Preço de venda deve ser maior que zero");
            }
            else if (precoVenda > 999999.99m)
            {
                erros.Add("Preço de venda não pode ser maior que R$ 999.999,99");
            }

            // Validação Margem de Lucro
            if (margemLucro < 0)
            {
                erros.Add("Margem de lucro não pode ser negativa");
            }
            else if (margemLucro > 9999.99m)
            {
                erros.Add("Margem de lucro não pode ser maior que 9999%");
            }

            // Validação de lógica de negócio: preço de venda deve ser maior que custo
            if (precoCusto > 0 && precoVenda > 0 && precoVenda <= precoCusto)
            {
                erros.Add("Preço de venda deve ser maior que o preço de custo");
            }

            // Validação Datas para produtos perecíveis
            if (dataVencimento > DateTime.MinValue && dataFabricao > DateTime.MinValue)
            {
                if (dataVencimento <= dataFabricao)
                {
                    erros.Add("Data de vencimento deve ser posterior à data de fabricação");
                }

                if (dataFabricao > DateTime.Now.AddDays(1))
                {
                    erros.Add("Data de fabricação não pode ser futura");
                }

                if (dataVencimento <= DateTime.Now)
                {
                    erros.Add("Produto com data de vencimento expirada não pode ser cadastrado");
                }
            }

            // Validação Quantidade
            if (quatidadeEstoqueProduto < 0)
            {
                erros.Add("Quantidade em estoque não pode ser negativa");
            }

            // Validação Fornecedor
            if (FornecedorId == Guid.Empty)
            {
                erros.Add("Fornecedor é obrigatório");
            }

            // Validação Categoria
            if (CategoriaId == Guid.Empty)
            {
                erros.Add("Categoria é obrigatória");
            }

            // Validação do ID quando presente
            if (Id != Guid.Empty && Id == Guid.Empty)
            {
                erros.Add("ID deve ser um GUID válido");
            }

            return erros;
        }

        // Método para normalizar dados
        public void NormalizarDados()
        {
            if (!string.IsNullOrEmpty(nomeProduto))
            {
                nomeProduto = System.Text.RegularExpressions.Regex.Replace(
                    nomeProduto.Trim(), @"\s+", " ");
                var textInfo = System.Globalization.CultureInfo.CurrentCulture.TextInfo;
                nomeProduto = textInfo.ToTitleCase(nomeProduto.ToLower());
            }

            if (!string.IsNullOrEmpty(descricaoProduto))
            {
                descricaoProduto = descricaoProduto.Trim();
            }

            if (!string.IsNullOrEmpty(codBarras))
            {
                codBarras = codBarras.Trim().Replace(" ", "").Replace("-", "");
            }

            // Recalcula margem de lucro se necessário
            if (precoCusto > 0 && precoVenda > 0)
            {
                margemLucro = ((precoVenda / precoCusto) - 1) * 100;
                margemLucro = Math.Round(margemLucro, 2);
            }
        }

        // Método para calcular margem de lucro
        public decimal CalcularMargemLucro()
        {
            if (precoCusto <= 0) return 0;
            return Math.Round(((precoVenda / precoCusto) - 1) * 100, 2);
        }

        // Método para calcular preço de venda baseado na margem
        public decimal CalcularPrecoVendaPorMargem(decimal margemDesejada)
        {
            if (precoCusto <= 0) return 0;
            return Math.Round(precoCusto * (1 + margemDesejada / 100), 2);
        }

        // Método para verificar se produto está com estoque baixo
        public bool EstoqueAbaixoMinimo(int estoqueMinimo = 10)
        {
            return quatidadeEstoqueProduto <= estoqueMinimo;
        }

        // Método para verificar se produto está próximo do vencimento
        public bool ProximoVencimento(int diasAlerta = 30)
        {
            if (!EhPerecivel) return false;
            return DiasVencimento <= diasAlerta && DiasVencimento > 0;
        }

        // Override ToString para melhor exibição
        public override string ToString()
        {
            return $"{nomeProduto} - {codBarras}";
        }

        // Método para verificar se é válido
        public bool EhValido()
        {
            return Validar().Count == 0;
        }

        // Método para obter status formatado
        public string GetStatusFormatado()
        {
            return statusAtivo == 1 ? "Ativo" : "Inativo";
        }

        // Método para obter informações de estoque
        public string GetInfoEstoque()
        {
            var info = $"Qtd: {quatidadeEstoqueProduto}";
            
            if (EstoqueMinimo)
                info += " (BAIXO)";
                
            if (EhPerecivel && ProximoVencimento())
                info += $" - Vence em {DiasVencimento} dias";
                
            return info;
        }
    }
}
