using System.ComponentModel.DataAnnotations;

namespace Sis_Pdv_Controle_Estoque_Form.Dto.Departamento
{
    public class DepartamentoDto
    {
        public string Id { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Nome do departamento é obrigatório")]
        [StringLength(150, MinimumLength = 2, ErrorMessage = "Nome deve ter entre 2 e 150 caracteres")]
        public string NomeDepartamento { get; set; } = string.Empty;

        // Propriedades adicionais para auditoria (se necessário)
        public DateTime? DataCriacao { get; set; }
        public DateTime? DataAtualizacao { get; set; }
        public bool Ativo { get; set; } = true;

        // Método para validar o DTO
        public List<string> Validar()
        {
            var erros = new List<string>();

            if (string.IsNullOrWhiteSpace(NomeDepartamento))
            {
                erros.Add("Nome do departamento é obrigatório");
            }
            else
            {
                var nome = NomeDepartamento.Trim();
                
                if (nome.Length < 2)
                {
                    erros.Add("Nome do departamento deve ter pelo menos 2 caracteres");
                }
                else if (nome.Length > 150)
                {
                    erros.Add("Nome do departamento não pode ter mais de 150 caracteres");
                }

                // Validação de caracteres especiais - versão mais simples
                if (nome.Any(c => !char.IsLetterOrDigit(c) && c != ' ' && c != '-' && c != '_' && c != '.'))
                {
                    erros.Add("Nome deve conter apenas letras, números, espaços e caracteres especiais básicos (-, _, .)");
                }

                // Validação de espaços consecutivos
                if (nome.Contains("  "))
                {
                    erros.Add("Nome não pode conter espaços consecutivos");
                }

                // Validação de início/fim com espaços
                if (nome != nome.Trim())
                {
                    erros.Add("Nome não pode começar ou terminar com espaços");
                }
            }

            // Validação do ID quando presente
            if (!string.IsNullOrWhiteSpace(Id) && !Guid.TryParse(Id, out _))
            {
                erros.Add("ID deve ser um GUID válido");
            }

            return erros;
        }

        // Método para normalizar o nome
        public void NormalizarNome()
        {
            if (!string.IsNullOrEmpty(NomeDepartamento))
            {
                // Remove espaços extras e normaliza
                NomeDepartamento = System.Text.RegularExpressions.Regex.Replace(
                    NomeDepartamento.Trim(), @"\s+", " ");
                
                // Converte para Title Case (primeira letra de cada palavra maiúscula)
                var textInfo = System.Globalization.CultureInfo.CurrentCulture.TextInfo;
                NomeDepartamento = textInfo.ToTitleCase(NomeDepartamento.ToLower());
            }
        }

        // Override ToString para melhor exibição
        public override string ToString()
        {
            return $"{NomeDepartamento} (ID: {Id})";
        }

        // Método para verificar se é válido
        public bool EhValido()
        {
            return Validar().Count == 0;
        }
    }
}
