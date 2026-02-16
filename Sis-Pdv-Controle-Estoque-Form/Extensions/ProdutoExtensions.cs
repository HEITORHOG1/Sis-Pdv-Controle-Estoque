using Sis_Pdv_Controle_Estoque_Form.Dto.Produto;

namespace Sis_Pdv_Controle_Estoque_Form.Extensions
{
    public static class ProdutoExtensions
    {
        /// <summary>
        /// Converte uma resposta da API para ProdutoDto
        /// </summary>
        public static ProdutoDto ToDto(this Data apiResponse)
        {
            if (apiResponse == null)
                return new ProdutoDto();

            return new ProdutoDto
            {
                Id = apiResponse.Id,
                codBarras = apiResponse.codBarras ?? string.Empty,
                nomeProduto = apiResponse.nomeProduto ?? string.Empty,
                descricaoProduto = apiResponse.descricaoProduto ?? string.Empty,
                precoCusto = apiResponse.precoCusto,
                precoVenda = apiResponse.precoVenda,
                margemLucro = apiResponse.margemLucro,
                dataFabricao = apiResponse.dataFabricao,
                dataVencimento = apiResponse.dataVencimento,
                quatidadeEstoqueProduto = apiResponse.quatidadeEstoqueProduto,
                // Note: API retorna nomes, não IDs, então precisamos mapear diferente
                statusAtivo = apiResponse.statusAtivo
            };
        }

        /// <summary>
        /// Converte uma lista de respostas da API para lista de ProdutoDto
        /// </summary>
        public static List<ProdutoDto> ToDtoList(this IEnumerable<Data> apiResponses)
        {
            if (apiResponses == null)
                return new List<ProdutoDto>();

            return apiResponses.Select(r => r.ToDto()).ToList();
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
                .Select(n => n.ToString())
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
                .Select(n => n.ToString())
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .ToList();
        }

        /// <summary>
        /// Formata preço para exibição em moeda brasileira
        /// </summary>
        public static string FormatPreco(this decimal valor)
        {
            return valor.ToString("C2", System.Globalization.CultureInfo.GetCultureInfo("pt-BR"));
        }

        /// <summary>
        /// Formata código de barras para exibição
        /// </summary>
        public static string FormatCodigoBarras(this string codigoBarras)
        {
            if (string.IsNullOrWhiteSpace(codigoBarras))
                return string.Empty;

            // Remove espaços e caracteres especiais
            var numbersOnly = new string(codigoBarras.Where(char.IsDigit).ToArray());

            // Formata código EAN-13 (13 dígitos)
            if (numbersOnly.Length == 13)
            {
                return $"{numbersOnly.Substring(0, 1)}-{numbersOnly.Substring(1, 6)}-{numbersOnly.Substring(7, 6)}";
            }
            // Formata código EAN-8 (8 dígitos)
            else if (numbersOnly.Length == 8)
            {
                return $"{numbersOnly.Substring(0, 4)}-{numbersOnly.Substring(4, 4)}";
            }

            return codigoBarras; // Retorna original se não for possível formatar
        }

        /// <summary>
        /// Verifica se o produto está ativo
        /// </summary>
        public static bool EstaAtivo(this ProdutoDto produto)
        {
            return produto?.statusAtivo == 1;
        }

        /// <summary>
        /// Obtém status formatado
        /// </summary>
        public static string GetStatusFormatado(this ProdutoDto produto)
        {
            return produto?.EstaAtivo() == true ? "Ativo" : "Inativo";
        }

        /// <summary>
        /// Verifica se o produto está com estoque baixo
        /// </summary>
        public static bool EstoqueAbaixoMinimo(this ProdutoDto produto, int estoqueMinimo = 10)
        {
            return produto?.quatidadeEstoqueProduto <= estoqueMinimo;
        }

        /// <summary>
        /// Obtém informações de alerta do produto
        /// </summary>
        public static List<string> GetAlertas(this ProdutoDto produto, int estoqueMinimo = 10, int diasVencimento = 30)
        {
            var alertas = new List<string>();

            if (produto == null) return alertas;

            // Alerta de estoque baixo
            if (produto.EstoqueAbaixoMinimo(estoqueMinimo))
            {
                alertas.Add($"Estoque baixo ({produto.quatidadeEstoqueProduto} unidades)");
            }

            // Alerta de produto próximo ao vencimento
            if (produto.EhPerecivel && produto.ProximoVencimento(diasVencimento))
            {
                alertas.Add($"Vence em {produto.DiasVencimento} dias");
            }

            // Alerta de produto vencido
            if (produto.EhPerecivel && produto.DiasVencimento <= 0)
            {
                alertas.Add("PRODUTO VENCIDO");
            }

            // Alerta de margem baixa
            if (produto.margemLucro < 10)
            {
                alertas.Add($"Margem baixa ({produto.margemLucro:F1}%)");
            }

            // Alerta de preço de venda menor que custo
            if (produto.precoVenda <= produto.precoCusto)
            {
                alertas.Add("Preço de venda menor que custo");
            }

            return alertas;
        }

        /// <summary>
        /// Obtém cor do alerta baseado na criticidade
        /// </summary>
        public static System.Drawing.Color GetCorAlerta(this ProdutoDto produto)
        {
            var alertas = produto.GetAlertas();

            if (alertas.Any(a => a.Contains("VENCIDO") || a.Contains("menor que custo")))
                return System.Drawing.Color.Red;

            if (alertas.Any(a => a.Contains("Vence em") || a.Contains("Estoque baixo")))
                return System.Drawing.Color.Orange;

            if (alertas.Any(a => a.Contains("Margem baixa")))
                return System.Drawing.Color.Yellow;

            return System.Drawing.Color.Green;
        }

        /// <summary>
        /// Calcula lucro bruto do produto
        /// </summary>
        public static decimal CalcularLucroBruto(this ProdutoDto produto)
        {
            if (produto == null) return 0;
            return produto.precoVenda - produto.precoCusto;
        }

        /// <summary>
        /// Calcula valor total do estoque
        /// </summary>
        public static decimal CalcularValorTotalEstoque(this ProdutoDto produto)
        {
            if (produto == null) return 0;
            return produto.precoCusto * produto.quatidadeEstoqueProduto;
        }

        /// <summary>
        /// Obtém informações completas de estoque formatadas
        /// </summary>
        public static string GetInfoEstoqueCompleta(this ProdutoDto produto)
        {
            if (produto == null) return string.Empty;

            var info = $"Qtd: {produto.quatidadeEstoqueProduto} | ";
            info += $"Valor: {produto.CalcularValorTotalEstoque().FormatPreco()}";

            var alertas = produto.GetAlertas();
            if (alertas.Any())
            {
                info += $" | ? {string.Join(", ", alertas)}";
            }

            return info;
        }

        /// <summary>
        /// Verifica se o produto pode ser vendido
        /// </summary>
        public static bool PodeSerVendido(this ProdutoDto produto)
        {
            if (produto == null) return false;

            return produto.EstaAtivo() && 
                   produto.quatidadeEstoqueProduto > 0 && 
                   (!produto.EhPerecivel || produto.DiasVencimento > 0);
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
                   $"Preço: {produto.precoVenda.FormatPreco()} | " +
                   $"Estoque: {produto.quatidadeEstoqueProduto} | " +
                   $"Margem: {produto.margemLucro:F1}%";
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
    }
}