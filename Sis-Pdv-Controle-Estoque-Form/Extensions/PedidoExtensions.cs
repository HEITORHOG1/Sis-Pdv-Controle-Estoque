using Sis_Pdv_Controle_Estoque_Form.Dto.Pedido;

namespace Sis_Pdv_Controle_Estoque_Form.Extensions
{
    public static class PedidoExtensions
    {
        /// <summary>
        /// Valida se uma resposta da API é válida
        /// </summary>
        public static bool IsValidResponse(this PedidoResponse response)
        {
            return response != null && response.success && response.data != null;
        }

        /// <summary>
        /// Valida se uma lista de respostas da API é válida
        /// </summary>
        public static bool IsValidResponse(this PedidoResponseList response)
        {
            return response != null && response.success && response.data != null;
        }

        /// <summary>
        /// Obtém as mensagens de erro de uma resposta
        /// </summary>
        public static List<string> GetErrorMessages(this PedidoResponse response)
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
        public static List<string> GetErrorMessages(this PedidoResponseList response)
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