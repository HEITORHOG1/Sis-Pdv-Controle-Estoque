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
        
        public bool isPerecivel { get; set; }
        
        [Required(ErrorMessage = "Fornecedor é obrigatório")]
        public Guid FornecedorId { get; set; }
        
        [Required(ErrorMessage = "Categoria é obrigatória")]
        public Guid CategoriaId { get; set; }
        
        [Range(0, 1, ErrorMessage = "Status deve ser 0 (inativo) ou 1 (ativo)")]
        public int statusAtivo { get; set; } = 1;

        // Propriedades adicionais para auditoria
        public DateTime? DataCriacao { get; set; }
        public DateTime? DataAtualizacao { get; set; }

        // Método para validar o DTO
        public List<string> Validar()
        {
            var erros = new List<string>();

            // Validação Código de Barras - Rigorosa (8-20 dígitos)
            if (string.IsNullOrWhiteSpace(codBarras))
            {
                erros.Add("Código de barras é obrigatório");
            }
            else
            {
                var codigoLimpo = codBarras.Trim();
                if (!System.Text.RegularExpressions.Regex.IsMatch(codigoLimpo, @"^[0-9]{8,20}$"))
                {
                    erros.Add("Código de barras deve conter apenas números e ter entre 8 e 20 dígitos");
                }
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

        // Método para obter tipo de produto formatado
        public string GetTipoFormatado()
        {
            return isPerecivel ? "Perecível" : "Não Perecível";
        }
    }
}
