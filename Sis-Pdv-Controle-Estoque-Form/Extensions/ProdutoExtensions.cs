using Sis_Pdv_Controle_Estoque_Form.Dto.Produto;

namespace Sis_Pdv_Controle_Estoque_Form.Extensions
{
    public static class ProdutoExtensions
    {
        /// <summary>
        /// Converte Data (resposta da API) para ProdutoDto
        /// </summary>
        public static ProdutoDto ToDto(this Data apiResponse)
        {
            if (apiResponse == null) return new ProdutoDto();

            return new ProdutoDto
            {
                Id = apiResponse.Id,
                codBarras = apiResponse.codBarras ?? string.Empty,
                nomeProduto = apiResponse.nomeProduto ?? string.Empty,
                descricaoProduto = apiResponse.descricaoProduto ?? string.Empty,
                isPerecivel = apiResponse.isPerecivel,
                FornecedorId = apiResponse.FornecedorId,
                CategoriaId = apiResponse.CategoriaId,
                statusAtivo = apiResponse.statusAtivo
            };
        }

        /// <summary>
        /// Verifica se o produto está ativo
        /// </summary>
        public static bool EstaAtivo(this ProdutoDto produto)
        {
            return produto?.statusAtivo == 1;
        }

        /// <summary>
        /// Obtém cor de alerta baseada em regras de negócio
        /// </summary>
        public static System.Drawing.Color GetCorAlerta(this ProdutoDto produto)
        {
            if (produto == null || !produto.EstaAtivo())
                return System.Drawing.Color.Gray;

            // Verde = normal (sem alertas)
            return System.Drawing.Color.Green;
        }

        /// <summary>
        /// Obtém lista de alertas do produto
        /// </summary>
        public static List<string> GetAlertas(this ProdutoDto produto)
        {
            var alertas = new List<string>();

            if (produto == null) return alertas;

            if (!produto.EstaAtivo())
                alertas.Add("Produto inativo");

            return alertas;
        }

        /// <summary>
        /// Formatar código de barras para exibição
        /// </summary>
        public static string FormatCodigoBarras(this string codigoBarras)
        {
            if (string.IsNullOrEmpty(codigoBarras)) return string.Empty;

            // Para códigos EAN-13 (13 dígitos), formatar como XXX-XXXX-XXXX-X
            if (codigoBarras.Length == 13)
            {
                return $"{codigoBarras.Substring(0, 3)}-{codigoBarras.Substring(3, 4)}-{codigoBarras.Substring(7, 4)}-{codigoBarras.Substring(11, 2)}";
            }

            // Para outros formatos, apenas retornar sem formatação
            return codigoBarras;
        }

        /// <summary>
        /// Obtém categoria de produto baseada no código de barras
        /// </summary>
        public static string GetCategoriaPorCodigoBarras(this ProdutoDto produto)
        {
            if (produto?.codBarras == null || produto.codBarras.Length < 3)
                return "Geral";

            var prefixo = produto.codBarras.Substring(0, 3);

            return prefixo switch
            {
                var p when p.StartsWith("789") => "Nacional",
                var p when p.StartsWith("001") => "Medicamento",
                var p when p.StartsWith("002") => "Alimento",
                var p when p.StartsWith("003") => "Bebida",
                var p when p.StartsWith("004") => "Limpeza",
                var p when p.StartsWith("005") => "Higiene",
                _ => "Geral"
            };
        }

        /// <summary>
        /// Gera código de barras sugerido baseado na categoria
        /// </summary>
        public static string GerarCodigoBarrasSugerido(this ProdutoDto produto, string categoria = "Geral")
        {
            var prefixo = categoria.ToLower() switch
            {
                "medicamento" => "001",
                "alimento" => "002",
                "bebida" => "003",
                "limpeza" => "004",
                "higiene" => "005",
                _ => "789" // Nacional genérico
            };

            var timestamp = DateTimeOffset.Now.ToUnixTimeSeconds().ToString();
            var sufixo = timestamp.Substring(timestamp.Length - 9); // Últimos 9 dígitos

            var codigoSemDigito = prefixo + sufixo;
            var digitoVerificador = CalcularDigitoVerificadorEAN13(codigoSemDigito);

            return codigoSemDigito + digitoVerificador;
        }

        /// <summary>
        /// Calcula dígito verificador para código EAN-13
        /// </summary>
        private static string CalcularDigitoVerificadorEAN13(string codigo)
        {
            if (codigo.Length != 12) return "0";

            int soma = 0;
            for (int i = 0; i < 12; i++)
            {
                int digito = int.Parse(codigo[i].ToString());
                soma += (i % 2 == 0) ? digito : digito * 3;
            }

            int resto = soma % 10;
            int digitoVerificador = (resto == 0) ? 0 : 10 - resto;

            return digitoVerificador.ToString();
        }

        /// <summary>
        /// Obtém resumo do produto para relatórios
        /// </summary>
        public static string GetResumo(this ProdutoDto produto)
        {
            if (produto == null) return string.Empty;

            return $"{produto.nomeProduto} | " +
                   $"Cód: {produto.codBarras.FormatCodigoBarras()} | " +
                   $"Tipo: {produto.GetTipoFormatado()} | " +
                   $"Status: {produto.GetStatusFormatado()}";
        }

        /// <summary>
        /// Verifica se é um produto sazonal baseado na data
        /// </summary>
        public static bool EhProdutoSazonal(this ProdutoDto produto)
        {
            if (produto?.nomeProduto == null) return false;

            var nome = produto.nomeProduto.ToLower();
            var mes = DateTime.Now.Month;

            // Produtos natalinos
            if ((mes == 11 || mes == 12) && (nome.Contains("natal") || nome.Contains("papai noel")))
                return true;

            // Produtos de páscoa
            if ((mes == 3 || mes == 4) && (nome.Contains("páscoa") || nome.Contains("chocolate") && nome.Contains("ovo")))
                return true;

            // Produtos de festa junina
            if ((mes == 6 || mes == 7) && (nome.Contains("junina") || nome.Contains("milho") || nome.Contains("festa")))
                return true;

            return false;
        }

        /// <summary>
        /// Valida se uma resposta da API é válida
        /// </summary>
        public static bool IsValidResponse(this ProdutoResponse response)
        {
            return response != null && response.success && response.data != null;
        }

        /// <summary>
        /// Valida se uma lista de respostas da API é válida
        /// </summary>
        public static bool IsValidResponse(this ProdutoResponseList response)
        {
            return response != null && response.success && response.data != null;
        }

        /// <summary>
        /// Obtém as mensagens de erro de uma resposta
        /// </summary>
        public static List<string> GetErrorMessages(this ProdutoResponse response)
        {
            if (response?.notifications == null)
                return new List<string> { "Erro desconhecido" };

            return response.notifications
                .Where(n => n != null)
                .Select(n => n.ToString() ?? string.Empty)
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .ToList();
        }

        /// <summary>
        /// Obtém as mensagens de erro de uma lista de respostas
        /// </summary>
        public static List<string> GetErrorMessages(this ProdutoResponseList response)
        {
            if (response?.notifications == null)
                return new List<string> { "Erro desconhecido" };

            return response.notifications
                .Where(n => n != null)
                .Select(n => n.ToString() ?? string.Empty)
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .ToList();
        }
    }
}