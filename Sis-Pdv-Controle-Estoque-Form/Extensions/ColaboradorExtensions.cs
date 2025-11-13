using Sis_Pdv_Controle_Estoque_Form.Dto.Colaborador;

namespace Sis_Pdv_Controle_Estoque_Form.Extensions
{
    public static class ColaboradorExtensions
    {
        /// <summary>
        /// Converte uma resposta da API para ColaboradorDto
        /// </summary>
        public static ColaboradorDto ToDto(this Data apiResponse)
        {
            if (apiResponse == null)
                return new ColaboradorDto();

            return new ColaboradorDto
            {
                id = apiResponse.id ?? string.Empty,
                nomeColaborador = apiResponse.nomeColaborador ?? string.Empty,
                departamentoId = apiResponse.Departamento?.Id ?? string.Empty,
                cpfColaborador = apiResponse.cpfColaborador ?? string.Empty,
                cargoColaborador = apiResponse.cargoColaborador ?? string.Empty,
                telefoneColaborador = apiResponse.telefoneColaborador ?? string.Empty,
                emailPessoalColaborador = apiResponse.emailPessoalColaborador ?? string.Empty,
                emailCorporativo = apiResponse.emailCorporativo ?? string.Empty,
                idlogin = apiResponse.usuario?.id ?? string.Empty,
                login = apiResponse.usuario?.login ?? string.Empty,
                senha = apiResponse.usuario?.senha ?? string.Empty,
                statusAtivo = apiResponse.usuario?.statusAtivo ?? false
            };
        }

        /// <summary>
        /// Converte uma lista de respostas da API para lista de ColaboradorDto
        /// </summary>
        public static List<ColaboradorDto> ToDtoList(this IEnumerable<Data> apiResponses)
        {
            if (apiResponses == null)
                return new List<ColaboradorDto>();

            return apiResponses.Select(r => r.ToDto()).ToList();
        }

        /// <summary>
        /// Valida se uma resposta da API é válida
        /// </summary>
        public static bool IsValidResponse(this ColaboradorResponse response)
        {
            return response != null && response.success && response.data != null;
        }

        /// <summary>
        /// Valida se uma lista de respostas da API é válida
        /// </summary>
        public static bool IsValidResponse(this ColaboradorResponseList response)
        {
            return response != null && response.success && response.data != null;
        }

        /// <summary>
        /// Obtém as mensagens de erro de uma resposta
        /// </summary>
        public static List<string> GetErrorMessages(this ColaboradorResponse response)
        {
            if (response?.notifications == null)
                return new List<string> { "Erro desconhecido" };

            return response.notifications
                .Where(n => n != null)
                .Select(n => n.ToString())
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .ToList();
        }

        /// <summary>
        /// Obtém as mensagens de erro de uma lista de respostas
        /// </summary>
        public static List<string> GetErrorMessages(this ColaboradorResponseList response)
        {
            if (response?.notifications == null)
                return new List<string> { "Erro desconhecido" };

            return response.notifications
                .Where(n => n != null)
                .Select(n => n.ToString())
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .ToList();
        }

        /// <summary>
        /// Formata CPF para exibição
        /// </summary>
        public static string FormatCPF(this string cpf)
        {
            if (string.IsNullOrWhiteSpace(cpf))
                return string.Empty;

            // Remove formatação existente
            var numbersOnly = new string(cpf.Where(char.IsDigit).ToArray());

            if (numbersOnly.Length == 11)
            {
                return $"{numbersOnly.Substring(0, 3)}.{numbersOnly.Substring(3, 3)}.{numbersOnly.Substring(6, 3)}-{numbersOnly.Substring(9, 2)}";
            }

            return cpf; // Retorna original se não for possível formatar
        }

        /// <summary>
        /// Formata telefone para exibição
        /// </summary>
        public static string FormatTelefone(this string telefone)
        {
            if (string.IsNullOrWhiteSpace(telefone))
                return string.Empty;

            // Remove formatação existente
            var numbersOnly = new string(telefone.Where(char.IsDigit).ToArray());

            if (numbersOnly.Length == 11)
            {
                return $"({numbersOnly.Substring(0, 2)}) {numbersOnly.Substring(2, 5)}-{numbersOnly.Substring(7, 4)}";
            }
            else if (numbersOnly.Length == 10)
            {
                return $"({numbersOnly.Substring(0, 2)}) {numbersOnly.Substring(2, 4)}-{numbersOnly.Substring(6, 4)}";
            }

            return telefone; // Retorna original se não for possível formatar
        }

        /// <summary>
        /// Verifica se o colaborador está ativo
        /// </summary>
        public static bool EstaAtivo(this ColaboradorDto colaborador)
        {
            return colaborador?.statusAtivo == true;
        }

        /// <summary>
        /// Obtém status formatado
        /// </summary>
        public static string GetStatusFormatado(this ColaboradorDto colaborador)
        {
            return colaborador?.EstaAtivo() == true ? "Ativo" : "Inativo";
        }

        /// <summary>
        /// Obtém informações de contato formatadas
        /// </summary>
        public static string GetContatoCompleto(this ColaboradorDto colaborador)
        {
            if (colaborador == null)
                return string.Empty;

            var contato = "";

            if (!string.IsNullOrWhiteSpace(colaborador.telefoneColaborador))
                contato += $"Tel: {colaborador.telefoneColaborador.FormatTelefone()}";

            if (!string.IsNullOrWhiteSpace(colaborador.emailPessoalColaborador))
            {
                if (!string.IsNullOrEmpty(contato)) contato += " | ";
                contato += $"Email: {colaborador.emailPessoalColaborador}";
            }

            if (!string.IsNullOrWhiteSpace(colaborador.emailCorporativo))
            {
                if (!string.IsNullOrEmpty(contato)) contato += " | ";
                contato += $"Corp: {colaborador.emailCorporativo}";
            }

            return contato;
        }

        /// <summary>
        /// Verifica se o colaborador pode ser excluído (validações de negócio)
        /// </summary>
        public static bool PodeSerExcluido(this ColaboradorDto colaborador)
        {
            // Adicione aqui regras de negócio específicas
            // Por exemplo: não pode excluir se tem vendas, se é admin único, etc.
            return colaborador != null;
        }

        /// <summary>
        /// Mascara senha para exibição
        /// </summary>
        public static string GetSenhaMascarada(this ColaboradorDto colaborador)
        {
            if (colaborador?.senha == null)
                return string.Empty;

            return new string('*', Math.Min(colaborador.senha.Length, 8));
        }

        /// <summary>
        /// Verifica se é administrador
        /// </summary>
        public static bool EhAdministrador(this ColaboradorDto colaborador)
        {
            if (colaborador?.cargoColaborador == null)
                return false;

            return colaborador.cargoColaborador.ToLower().Contains("admin") ||
                   colaborador.cargoColaborador.ToLower().Contains("gerente") ||
                   colaborador.cargoColaborador.ToLower().Contains("supervisor");
        }

        /// <summary>
        /// Gera nome de usuário sugerido baseado no nome
        /// </summary>
        public static string GerarLoginSugerido(this ColaboradorDto colaborador)
        {
            if (string.IsNullOrWhiteSpace(colaborador?.nomeColaborador))
                return string.Empty;

            var nomes = colaborador.nomeColaborador.Trim().Split(' ');
            if (nomes.Length == 1)
            {
                return nomes[0].ToLower();
            }
            else if (nomes.Length >= 2)
            {
                return $"{nomes[0]}.{nomes[nomes.Length - 1]}".ToLower();
            }

            return colaborador.nomeColaborador.Replace(" ", ".").ToLower();
        }

        /// <summary>
        /// Gera email corporativo sugerido baseado no nome
        /// </summary>
        public static string GerarEmailCorporativoSugerido(this ColaboradorDto colaborador, string dominioEmpresa = "empresa.com.br")
        {
            if (string.IsNullOrWhiteSpace(colaborador?.nomeColaborador))
                return string.Empty;

            var nomes = colaborador.nomeColaborador.Trim().ToLower().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            
            if (nomes.Length == 1)
            {
                return $"{nomes[0]}@{dominioEmpresa}";
            }
            else if (nomes.Length >= 2)
            {
                // Remove acentos e caracteres especiais
                var primeiroNome = RemoverAcentos(nomes[0]);
                var ultimoNome = RemoverAcentos(nomes[nomes.Length - 1]);
                
                return $"{primeiroNome}.{ultimoNome}@{dominioEmpresa}";
            }

            return $"{RemoverAcentos(colaborador.nomeColaborador.Replace(" ", ".").ToLower())}@{dominioEmpresa}";
        }

        /// <summary>
        /// Remove acentos de uma string
        /// </summary>
        private static string RemoverAcentos(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
                return string.Empty;

            var normalizedString = texto.Normalize(System.Text.NormalizationForm.FormD);
            var stringBuilder = new System.Text.StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = System.Globalization.CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != System.Globalization.UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(System.Text.NormalizationForm.FormC);
        }

        /// <summary>
        /// Valida se os emails são diferentes
        /// </summary>
        public static bool EmailsSaoDiferentes(this ColaboradorDto colaborador)
        {
            if (string.IsNullOrWhiteSpace(colaborador?.emailPessoalColaborador) || 
                string.IsNullOrWhiteSpace(colaborador?.emailCorporativo))
                return true;

            return !string.Equals(
                colaborador.emailPessoalColaborador.Trim(), 
                colaborador.emailCorporativo.Trim(), 
                StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Obtém sugestões de domínios corporativos comuns
        /// </summary>
        public static List<string> GetDominiosCorporativosComuns()
        {
            return new List<string>
            {
                "empresa.com.br",
                "companhia.com.br", 
                "corporation.com",
                "corp.com.br",
                "group.com.br",
                "ltda.com.br",
                "sa.com.br"
            };
        }
    }
}