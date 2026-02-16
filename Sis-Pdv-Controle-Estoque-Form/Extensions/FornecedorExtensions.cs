using Sis_Pdv_Controle_Estoque_Form.Dto.Fornecedor;

namespace Sis_Pdv_Controle_Estoque_Form.Extensions
{
    public static class FornecedorExtensions
    {
        /// <summary>
        /// Converte uma resposta da API para FornecedorDto
        /// </summary>
        public static FornecedorDto ToDto(this Data apiResponse)
        {
            if (apiResponse == null)
                return new FornecedorDto();

            return new FornecedorDto
            {
                Id = apiResponse.id ?? string.Empty,
                nomeFantasia = apiResponse.nomeFantasia ?? string.Empty,
                Cnpj = apiResponse.Cnpj ?? string.Empty,
                inscricaoEstadual = apiResponse.inscricaoEstadual ?? string.Empty,
                cepFornecedor = apiResponse.cepFornecedor,
                Rua = apiResponse.Rua ?? string.Empty,
                Numero = apiResponse.Numero ?? string.Empty,
                Complemento = apiResponse.Complemento ?? string.Empty,
                Bairro = apiResponse.Bairro ?? string.Empty,
                Cidade = apiResponse.Cidade ?? string.Empty,
                Uf = apiResponse.Uf ?? string.Empty,
                statusAtivo = apiResponse.statusAtivo
            };
        }

        /// <summary>
        /// Converte uma lista de respostas da API para lista de FornecedorDto
        /// </summary>
        public static List<FornecedorDto> ToDtoList(this IEnumerable<Data> apiResponses)
        {
            if (apiResponses == null)
                return new List<FornecedorDto>();

            return apiResponses.Select(r => r.ToDto()).ToList();
        }

        /// <summary>
        /// Valida se uma resposta da API é válida
        /// </summary>
        public static bool IsValidResponse(this FornecedorResponse response)
        {
            return response != null && response.success && response.data != null;
        }

        /// <summary>
        /// Valida se uma lista de respostas da API é válida
        /// </summary>
        public static bool IsValidResponse(this FornecedorResponseList response)
        {
            return response != null && response.success && response.data != null;
        }

        /// <summary>
        /// Obtém as mensagens de erro de uma resposta
        /// </summary>
        public static List<string> GetErrorMessages(this FornecedorResponse response)
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
        public static List<string> GetErrorMessages(this FornecedorResponseList response)
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
        /// Formata CNPJ para exibição
        /// </summary>
        public static string FormatCNPJ(this string cnpj)
        {
            if (string.IsNullOrWhiteSpace(cnpj))
                return string.Empty;

            // Remove formatação existente
            var numbersOnly = new string(cnpj.Where(char.IsDigit).ToArray());

            if (numbersOnly.Length == 14)
            {
                return $"{numbersOnly.Substring(0, 2)}.{numbersOnly.Substring(2, 3)}.{numbersOnly.Substring(5, 3)}/{numbersOnly.Substring(8, 4)}-{numbersOnly.Substring(12, 2)}";
            }

            return cnpj; // Retorna original se não for possível formatar
        }

        /// <summary>
        /// Formata CEP para exibição
        /// </summary>
        public static string FormatCEP(this int cep)
        {
            var cepString = cep.ToString("D8");
            return $"{cepString.Substring(0, 5)}-{cepString.Substring(5, 3)}";
        }

        /// <summary>
        /// Obtém endereço completo formatado
        /// </summary>
        public static string GetEnderecoCompleto(this FornecedorDto fornecedor)
        {
            if (fornecedor == null)
                return string.Empty;

            var endereco = fornecedor.Rua;

            if (!string.IsNullOrWhiteSpace(fornecedor.Numero))
                endereco += $", {fornecedor.Numero}";

            if (!string.IsNullOrWhiteSpace(fornecedor.Complemento))
                endereco += $" - {fornecedor.Complemento}";

            endereco += $", {fornecedor.Bairro}";
            endereco += $", {fornecedor.Cidade} - {fornecedor.Uf}";
            endereco += $", CEP: {fornecedor.cepFornecedor.FormatCEP()}";

            return endereco;
        }

        /// <summary>
        /// Verifica se o fornecedor está ativo
        /// </summary>
        public static bool EstaAtivo(this FornecedorDto fornecedor)
        {
            return fornecedor?.statusAtivo == 1;
        }

        /// <summary>
        /// Obtém status formatado
        /// </summary>
        public static string GetStatusFormatado(this FornecedorDto fornecedor)
        {
            return fornecedor?.EstaAtivo() == true ? "Ativo" : "Inativo";
        }
    }
}