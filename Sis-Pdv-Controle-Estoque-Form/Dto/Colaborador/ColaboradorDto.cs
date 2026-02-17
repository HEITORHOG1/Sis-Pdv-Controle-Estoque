using System.ComponentModel.DataAnnotations;

namespace Sis_Pdv_Controle_Estoque_Form.Dto.Colaborador
{
    public class ColaboradorDto
    {
        public string id { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Nome do colaborador é obrigatório")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Nome deve ter entre 2 e 100 caracteres")]
        public string NomeColaborador { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Departamento é obrigatório")]
        public string departamentoId { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "CPF é obrigatório")]
        [StringLength(14, MinimumLength = 11, ErrorMessage = "CPF deve ter formato válido")]
        public string cpfColaborador { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Cargo é obrigatório")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Cargo deve ter entre 2 e 50 caracteres")]
        public string cargoColaborador { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Telefone é obrigatório")]
        [StringLength(15, MinimumLength = 10, ErrorMessage = "Telefone deve ter entre 10 e 15 caracteres")]
        public string telefoneColaborador { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Email pessoal é obrigatório")]
        [EmailAddress(ErrorMessage = "Email pessoal deve ter formato válido")]
        public string emailPessoalColaborador { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Email corporativo é obrigatório")]
        [EmailAddress(ErrorMessage = "Email corporativo deve ter formato válido")]
        public string emailCorporativo { get; set; } = string.Empty;
        
        public string idlogin { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Login é obrigatório")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Login deve ter entre 3 e 20 caracteres")]
        public string login { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Senha é obrigatória")]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "Senha deve ter entre 6 e 50 caracteres")]
        public string senha { get; set; } = string.Empty;
        
        public bool StatusAtivo { get; set; } = true;

        // Propriedades adicionais para auditoria
        public DateTime? DataCriacao { get; set; }
        public DateTime? DataAtualizacao { get; set; }

        // Método para validar o DTO
        public List<string> Validar()
        {
            var erros = new List<string>();

            // Validação Nome
            if (string.IsNullOrWhiteSpace(NomeColaborador))
            {
                erros.Add("Nome do colaborador é obrigatório");
            }
            else if (NomeColaborador.Trim().Length < 2)
            {
                erros.Add("Nome deve ter pelo menos 2 caracteres");
            }
            else if (NomeColaborador.Length > 100)
            {
                erros.Add("Nome não pode ter mais de 100 caracteres");
            }
            else if (!System.Text.RegularExpressions.Regex.IsMatch(NomeColaborador, @"^[a-zA-ZÀ-ÿ\s]+$"))
            {
                erros.Add("Nome deve conter apenas letras e espaços");
            }

            // Validação CPF
            if (string.IsNullOrWhiteSpace(cpfColaborador))
            {
                erros.Add("CPF é obrigatório");
            }
            else if (!ValidarCPF(cpfColaborador))
            {
                erros.Add("CPF deve ter formato válido");
            }

            // Validação Departamento
            if (string.IsNullOrWhiteSpace(departamentoId))
            {
                erros.Add("Departamento é obrigatório");
            }
            else if (!Guid.TryParse(departamentoId, out _))
            {
                erros.Add("Departamento deve ser um ID válido");
            }

            // Validação Cargo
            if (string.IsNullOrWhiteSpace(cargoColaborador))
            {
                erros.Add("Cargo é obrigatório");
            }
            else if (cargoColaborador.Trim().Length < 2)
            {
                erros.Add("Cargo deve ter pelo menos 2 caracteres");
            }
            else if (cargoColaborador.Length > 50)
            {
                erros.Add("Cargo não pode ter mais de 50 caracteres");
            }

            // Validação Telefone
            if (string.IsNullOrWhiteSpace(telefoneColaborador))
            {
                erros.Add("Telefone é obrigatório");
            }
            else if (!ValidarTelefone(telefoneColaborador))
            {
                erros.Add("Telefone deve ter formato válido");
            }

            // Validação Email Pessoal
            if (string.IsNullOrWhiteSpace(emailPessoalColaborador))
            {
                erros.Add("Email pessoal é obrigatório");
            }
            else if (!ValidarEmail(emailPessoalColaborador))
            {
                erros.Add("Email pessoal deve ter formato válido");
            }

            // Validação Email Corporativo
            if (string.IsNullOrWhiteSpace(emailCorporativo))
            {
                erros.Add("Email corporativo é obrigatório");
            }
            else if (!ValidarEmail(emailCorporativo))
            {
                erros.Add("Email corporativo deve ter formato válido");
            }

            // NOVA VALIDAÇÃO: Emails devem ser diferentes
            if (!string.IsNullOrWhiteSpace(emailPessoalColaborador) && 
                !string.IsNullOrWhiteSpace(emailCorporativo) &&
                string.Equals(emailPessoalColaborador.Trim(), emailCorporativo.Trim(), StringComparison.OrdinalIgnoreCase))
            {
                erros.Add("E-mail corporativo deve ser diferente do e-mail pessoal");
            }

            // Validação Login
            if (string.IsNullOrWhiteSpace(login))
            {
                erros.Add("Login é obrigatório");
            }
            else if (login.Trim().Length < 3)
            {
                erros.Add("Login deve ter pelo menos 3 caracteres");
            }
            else if (login.Length > 20)
            {
                erros.Add("Login não pode ter mais de 20 caracteres");
            }
            else if (!System.Text.RegularExpressions.Regex.IsMatch(login, @"^[a-zA-Z0-9._]+$"))
            {
                erros.Add("Login deve conter apenas letras, números, pontos e underscores");
            }

            // Validação Senha
            if (string.IsNullOrWhiteSpace(senha))
            {
                erros.Add("Senha é obrigatória");
            }
            else if (senha.Length < 6)
            {
                erros.Add("Senha deve ter pelo menos 6 caracteres");
            }
            else if (senha.Length > 50)
            {
                erros.Add("Senha não pode ter mais de 50 caracteres");
            }

            // Validação do ID quando presente
            if (!string.IsNullOrWhiteSpace(id) && !Guid.TryParse(id, out _))
            {
                erros.Add("ID deve ser um GUID válido");
            }

            return erros;
        }

        // Método para normalizar dados
        public void NormalizarDados()
        {
            if (!string.IsNullOrEmpty(NomeColaborador))
            {
                NomeColaborador = System.Text.RegularExpressions.Regex.Replace(
                    NomeColaborador.Trim(), @"\s+", " ");
                var textInfo = System.Globalization.CultureInfo.CurrentCulture.TextInfo;
                NomeColaborador = textInfo.ToTitleCase(NomeColaborador.ToLower());
            }

            if (!string.IsNullOrEmpty(cargoColaborador))
            {
                var textInfo = System.Globalization.CultureInfo.CurrentCulture.TextInfo;
                cargoColaborador = textInfo.ToTitleCase(cargoColaborador.Trim().ToLower());
            }

            if (!string.IsNullOrEmpty(emailPessoalColaborador))
            {
                emailPessoalColaborador = emailPessoalColaborador.Trim().ToLower();
            }

            if (!string.IsNullOrEmpty(emailCorporativo))
            {
                emailCorporativo = emailCorporativo.Trim().ToLower();
            }

            if (!string.IsNullOrEmpty(login))
            {
                login = login.Trim().ToLower();
            }

            if (!string.IsNullOrEmpty(cpfColaborador))
            {
                cpfColaborador = cpfColaborador.Replace(".", "").Replace("-", "").Replace(" ", "");
                if (cpfColaborador.Length == 11)
                {
                    cpfColaborador = $"{cpfColaborador.Substring(0, 3)}.{cpfColaborador.Substring(3, 3)}.{cpfColaborador.Substring(6, 3)}-{cpfColaborador.Substring(9, 2)}";
                }
            }

            if (!string.IsNullOrEmpty(telefoneColaborador))
            {
                telefoneColaborador = telefoneColaborador.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "");
                if (telefoneColaborador.Length == 11)
                {
                    telefoneColaborador = $"({telefoneColaborador.Substring(0, 2)}) {telefoneColaborador.Substring(2, 5)}-{telefoneColaborador.Substring(7, 4)}";
                }
                else if (telefoneColaborador.Length == 10)
                {
                    telefoneColaborador = $"({telefoneColaborador.Substring(0, 2)}) {telefoneColaborador.Substring(2, 4)}-{telefoneColaborador.Substring(6, 4)}";
                }
            }
        }

        // Validação de CPF
        private bool ValidarCPF(string cpf)
        {
            if (string.IsNullOrWhiteSpace(cpf))
                return false;

            // Remove formatação
            cpf = cpf.Replace(".", "").Replace("-", "").Replace(" ", "");

            // Verifica se tem 11 dígitos
            if (cpf.Length != 11)
                return false;

            // Verifica se todos os caracteres são dígitos
            if (!cpf.All(char.IsDigit))
                return false;

            // Verifica se não é uma sequência de números iguais
            if (cpf.All(c => c == cpf[0]))
                return false;

            // Validação do algoritmo do CPF
            int[] multiplicador1 = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            string tempCpf = cpf.Substring(0, 9);
            int soma = 0;

            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];

            int resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            string digito = resto.ToString();
            tempCpf = tempCpf + digito;
            soma = 0;

            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];

            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            digito = digito + resto.ToString();

            return cpf.EndsWith(digito);
        }

        // Validação de Email
        private bool ValidarEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        // Validação de Telefone
        private bool ValidarTelefone(string telefone)
        {
            if (string.IsNullOrWhiteSpace(telefone))
                return false;

            // Remove formatação
            telefone = telefone.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "");

            // Verifica se tem 10 ou 11 dígitos
            if (telefone.Length != 10 && telefone.Length != 11)
                return false;

            // Verifica se todos são dígitos
            if (!telefone.All(char.IsDigit))
                return false;

            return true;
        }

        // Override ToString para melhor exibição
        public override string ToString()
        {
            return $"{NomeColaborador} - {cargoColaborador}";
        }

        // Método para verificar se é válido
        public bool EhValido()
        {
            return Validar().Count == 0;
        }
    }
}
