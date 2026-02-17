using System.ComponentModel.DataAnnotations;

namespace Sis_Pdv_Controle_Estoque_Form.Dto.Fornecedor
{
    public class FornecedorDto
    {
        public string Id { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Nome fantasia é obrigatório")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Nome fantasia deve ter entre 2 e 100 caracteres")]
        public string NomeFantasia { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "CNPJ é obrigatório")]
        public string Cnpj { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Inscrição estadual é obrigatória")]
        [StringLength(15, MinimumLength = 8, ErrorMessage = "Inscrição estadual deve ter entre 8 e 15 caracteres")]
        public string InscricaoEstadual { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "CEP é obrigatório")]
        [Range(10000000, 99999999, ErrorMessage = "CEP deve ter 8 dígitos")]
        public int CepFornecedor { get; set; }
        
        [Required(ErrorMessage = "Rua é obrigatória")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Rua deve ter entre 5 e 100 caracteres")]
        public string Rua { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Número é obrigatório")]
        [StringLength(10, MinimumLength = 1, ErrorMessage = "Número deve ter entre 1 e 10 caracteres")]
        public string Numero { get; set; } = string.Empty;
        
        [StringLength(50, ErrorMessage = "Complemento não pode ter mais de 50 caracteres")]
        public string Complemento { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Bairro é obrigatório")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Bairro deve ter entre 2 e 50 caracteres")]
        public string Bairro { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Cidade é obrigatória")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Cidade deve ter entre 2 e 50 caracteres")]
        public string Cidade { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "UF é obrigatória")]
        [StringLength(2, MinimumLength = 2, ErrorMessage = "UF deve ter 2 caracteres")]
        public string Uf { get; set; } = string.Empty;
        
        [Range(0, 1, ErrorMessage = "Status deve ser 0 (inativo) ou 1 (ativo)")]
        public int StatusAtivo { get; set; } = 1;

        // Propriedades adicionais para auditoria
        public DateTime? DataCriacao { get; set; }
        public DateTime? DataAtualizacao { get; set; }

        // Lista de UFs válidas para validação
        private static readonly string[] UfsValidas = 
        {
            "AC", "AL", "AP", "AM", "BA", "CE", "DF", "ES", "GO", "MA", 
            "MT", "MS", "MG", "PA", "PB", "PR", "PE", "PI", "RJ", "RN", 
            "RS", "RO", "RR", "SC", "SP", "SE", "TO"
        };

        // Método para validar o DTO
        public List<string> Validar()
        {
            var erros = new List<string>();

            // Validação Nome Fantasia
            if (string.IsNullOrWhiteSpace(NomeFantasia))
            {
                erros.Add("Nome fantasia é obrigatório");
            }
            else if (NomeFantasia.Trim().Length < 2)
            {
                erros.Add("Nome fantasia deve ter pelo menos 2 caracteres");
            }
            else if (NomeFantasia.Length > 100)
            {
                erros.Add("Nome fantasia não pode ter mais de 100 caracteres");
            }
            else if (!System.Text.RegularExpressions.Regex.IsMatch(NomeFantasia, @"^[a-zA-ZÀ-ÿ0-9\s\-\.&]+$"))
            {
                erros.Add("Nome fantasia contém caracteres inválidos");
            }

            // Validação CNPJ
            if (string.IsNullOrWhiteSpace(Cnpj))
            {
                erros.Add("CNPJ é obrigatório");
            }
            else if (!ValidarCNPJ(Cnpj))
            {
                erros.Add("CNPJ deve ter formato válido");
            }

            // Validação Inscrição Estadual
            if (string.IsNullOrWhiteSpace(InscricaoEstadual))
            {
                erros.Add("Inscrição estadual é obrigatória");
            }
            else if (InscricaoEstadual.Length < 8 || InscricaoEstadual.Length > 15)
            {
                erros.Add("Inscrição estadual deve ter entre 8 e 15 caracteres");
            }
            else if (!System.Text.RegularExpressions.Regex.IsMatch(InscricaoEstadual, @"^[0-9]+$"))
            {
                erros.Add("Inscrição estadual deve conter apenas números");
            }

            // Validação CEP
            if (CepFornecedor < 10000000 || CepFornecedor > 99999999)
            {
                erros.Add("CEP deve ter 8 dígitos válidos");
            }

            // Validação Rua
            if (string.IsNullOrWhiteSpace(Rua))
            {
                erros.Add("Rua é obrigatória");
            }
            else if (Rua.Trim().Length < 5)
            {
                erros.Add("Rua deve ter pelo menos 5 caracteres");
            }
            else if (Rua.Length > 100)
            {
                erros.Add("Rua não pode ter mais de 100 caracteres");
            }

            // Validação Número
            if (string.IsNullOrWhiteSpace(Numero))
            {
                erros.Add("Número é obrigatório");
            }
            else if (Numero.Trim().Length < 1 || Numero.Length > 10)
            {
                erros.Add("Número deve ter entre 1 e 10 caracteres");
            }
            else if (!System.Text.RegularExpressions.Regex.IsMatch(Numero, @"^[0-9A-Za-z\s\-]+$"))
            {
                erros.Add("Número do endereço inválido");
            }

            // Validação Bairro
            if (string.IsNullOrWhiteSpace(Bairro))
            {
                erros.Add("Bairro é obrigatório");
            }
            else if (Bairro.Trim().Length < 2)
            {
                erros.Add("Bairro deve ter pelo menos 2 caracteres");
            }
            else if (Bairro.Length > 50)
            {
                erros.Add("Bairro não pode ter mais de 50 caracteres");
            }

            // Validação Cidade
            if (string.IsNullOrWhiteSpace(Cidade))
            {
                erros.Add("Cidade é obrigatória");
            }
            else if (Cidade.Trim().Length < 2)
            {
                erros.Add("Cidade deve ter pelo menos 2 caracteres");
            }
            else if (Cidade.Length > 50)
            {
                erros.Add("Cidade não pode ter mais de 50 caracteres");
            }

            // Validação UF
            if (string.IsNullOrWhiteSpace(Uf))
            {
                erros.Add("UF é obrigatória");
            }
            else if (Uf.Length != 2)
            {
                erros.Add("UF deve ter exatamente 2 caracteres");
            }
            else if (!UfsValidas.Contains(Uf.ToUpper()))
            {
                erros.Add("UF inválida");
            }

            // Validação Complemento (opcional)
            if (!string.IsNullOrEmpty(Complemento) && Complemento.Length > 50)
            {
                erros.Add("Complemento não pode ter mais de 50 caracteres");
            }

            // Validação do ID quando presente
            if (!string.IsNullOrWhiteSpace(Id) && !Guid.TryParse(Id, out _))
            {
                erros.Add("ID deve ser um GUID válido");
            }

            return erros;
        }

        // Método para normalizar dados
        public void NormalizarDados()
        {
            if (!string.IsNullOrEmpty(NomeFantasia))
            {
                NomeFantasia = System.Text.RegularExpressions.Regex.Replace(
                    NomeFantasia.Trim(), @"\s+", " ");
                var textInfo = System.Globalization.CultureInfo.CurrentCulture.TextInfo;
                NomeFantasia = textInfo.ToTitleCase(NomeFantasia.ToLower());
            }

            if (!string.IsNullOrEmpty(Rua))
            {
                Rua = System.Text.RegularExpressions.Regex.Replace(Rua.Trim(), @"\s+", " ");
            }

            if (!string.IsNullOrEmpty(Numero))
            {
                Numero = Numero.Trim().ToUpper();
            }

            if (!string.IsNullOrEmpty(Bairro))
            {
                var textInfo = System.Globalization.CultureInfo.CurrentCulture.TextInfo;
                Bairro = textInfo.ToTitleCase(Bairro.Trim().ToLower());
            }

            if (!string.IsNullOrEmpty(Cidade))
            {
                var textInfo = System.Globalization.CultureInfo.CurrentCulture.TextInfo;
                Cidade = textInfo.ToTitleCase(Cidade.Trim().ToLower());
            }

            if (!string.IsNullOrEmpty(Uf))
            {
                Uf = Uf.Trim().ToUpper();
            }

            if (!string.IsNullOrEmpty(InscricaoEstadual))
            {
                InscricaoEstadual = InscricaoEstadual.Replace(".", "").Replace("-", "").Replace(" ", "");
            }

            if (!string.IsNullOrEmpty(Cnpj))
            {
                Cnpj = Cnpj.Replace(".", "").Replace("/", "").Replace("-", "").Replace(" ", "");
                if (Cnpj.Length == 14)
                {
                    Cnpj = $"{Cnpj.Substring(0, 2)}.{Cnpj.Substring(2, 3)}.{Cnpj.Substring(5, 3)}/{Cnpj.Substring(8, 4)}-{Cnpj.Substring(12, 2)}";
                }
            }
        }

        // Validação de CNPJ
        private bool ValidarCNPJ(string cnpj)
        {
            if (string.IsNullOrWhiteSpace(cnpj))
                return false;

            // Remove formatação
            cnpj = cnpj.Replace(".", "").Replace("/", "").Replace("-", "").Replace(" ", "");

            // Verifica se tem 14 dígitos
            if (cnpj.Length != 14)
                return false;

            // Verifica se todos os caracteres são dígitos
            if (!cnpj.All(char.IsDigit))
                return false;

            // Verifica se não é uma sequência de números iguais
            if (cnpj.All(c => c == cnpj[0]))
                return false;

            // Validação do algoritmo do CNPJ
            int[] multiplicador1 = { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

            string tempCnpj = cnpj.Substring(0, 12);
            int soma = 0;

            for (int i = 0; i < 12; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador1[i];

            int resto = (soma % 11);
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            string digito = resto.ToString();
            tempCnpj = tempCnpj + digito;
            soma = 0;

            for (int i = 0; i < 13; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador2[i];

            resto = (soma % 11);
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            digito = digito + resto.ToString();

            return cnpj.EndsWith(digito);
        }

        // Override ToString para melhor exibição
        public override string ToString()
        {
            return $"{NomeFantasia} - CNPJ: {Cnpj}";
        }

        // Método para verificar se é válido
        public bool EhValido()
        {
            return Validar().Count == 0;
        }
    }
}
