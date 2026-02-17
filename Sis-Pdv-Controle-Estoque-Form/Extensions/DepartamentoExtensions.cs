using Sis_Pdv_Controle_Estoque_Form.Dto.Departamento;

namespace Sis_Pdv_Controle_Estoque_Form.Extensions
{
    public static class DepartamentoExtensions
    {
        /// <summary>
        /// Converte uma resposta da API para DepartamentoDto
        /// </summary>
        public static DepartamentoDto ToDto(this Data apiResponse)
        {
            if (apiResponse == null)
                return new DepartamentoDto();

            return new DepartamentoDto
            {
                Id = apiResponse.id ?? string.Empty,
                NomeDepartamento = apiResponse.NomeDepartamento ?? string.Empty
            };
        }

        /// <summary>
        /// Converte uma lista de respostas da API para lista de DepartamentoDto
        /// </summary>
        public static List<DepartamentoDto> ToDtoList(this IEnumerable<Data> apiResponses)
        {
            if (apiResponses == null)
                return new List<DepartamentoDto>();

            return apiResponses.Select(r => r.ToDto()).ToList();
        }

        /// <summary>
        /// Valida se uma resposta da API � v�lida
        /// </summary>
        public static bool IsValidResponse(this DepartamentoResponse response)
        {
            return response != null && response.success && response.data != null;
        }

        /// <summary>
        /// Valida se uma lista de respostas da API � v�lida
        /// </summary>
        public static bool IsValidResponse(this DepartamentoResponseList response)
        {
            return response != null && response.success && response.data != null;
        }

        /// <summary>
        /// Obt�m as mensagens de erro de uma resposta
        /// </summary>
        public static List<string> GetErrorMessages(this DepartamentoResponse response)
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
        /// Obt�m as mensagens de erro de uma lista de respostas
        /// </summary>
        public static List<string> GetErrorMessages(this DepartamentoResponseList response)
        {
            if (response?.notifications == null)
                return new List<string> { "Erro desconhecido" };

            return response.notifications
                .Where(n => n != null)
                .Select(n => n.ToString())
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .ToList();
        }
    }
}