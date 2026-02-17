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
                CodBarras = apiResponse.CodBarras ?? string.Empty,
                NomeProduto = apiResponse.NomeProduto ?? string.Empty,
                DescricaoProduto = apiResponse.DescricaoProduto ?? string.Empty,
                PrecoCusto = apiResponse.PrecoCusto,
                PrecoVenda = apiResponse.PrecoVenda,
                MargemLucro = apiResponse.MargemLucro,
                DataFabricao = apiResponse.DataFabricao,
                DataVencimento = apiResponse.DataVencimento,
                QuantidadeEstoqueProduto = apiResponse.QuantidadeEstoqueProduto,
                // Note: API retorna nomes, n�o IDs, ent�o precisamos mapear diferente
                StatusAtivo = apiResponse.StatusAtivo
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
        /// Valida se uma resposta da API � v�lida
        /// </summary>
        public static bool IsValidResponse(this ProdutoResponse response)
        {
            return response != null && response.success && response.data != null;
        }

        /// <summary>
        /// Valida se uma lista de respostas da API � v�lida
        /// </summary>
        public static bool IsValidResponse(this ProdutoResponseList response)
        {
            return response != null && response.success && response.data != null;
        }

        /// <summary>
        /// Obt�m as mensagens de erro de uma resposta
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
        /// Obt�m as mensagens de erro de uma lista de respostas
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
        /// Formata pre�o para exibi��o em moeda brasileira
        /// </summary>
        public static string FormatPreco(this decimal valor)
        {
            return valor.ToString("C2", System.Globalization.CultureInfo.GetCultureInfo("pt-BR"));
        }

        /// <summary>
        /// Formata c�digo de barras para exibi��o
        /// </summary>
        public static string FormatCodigoBarras(this string codigoBarras)
        {
            if (string.IsNullOrWhiteSpace(codigoBarras))
                return string.Empty;

            // Remove espa�os e caracteres especiais
            var numbersOnly = new string(codigoBarras.Where(char.IsDigit).ToArray());

            // Formata c�digo EAN-13 (13 d�gitos)
            if (numbersOnly.Length == 13)
            {
                return $"{numbersOnly.Substring(0, 1)}-{numbersOnly.Substring(1, 6)}-{numbersOnly.Substring(7, 6)}";
            }
            // Formata c�digo EAN-8 (8 d�gitos)
            else if (numbersOnly.Length == 8)
            {
                return $"{numbersOnly.Substring(0, 4)}-{numbersOnly.Substring(4, 4)}";
            }

            return codigoBarras; // Retorna original se n�o for poss�vel formatar
        }

        /// <summary>
        /// Verifica se o produto est� ativo
        /// </summary>
        public static bool EstaAtivo(this ProdutoDto produto)
        {
            return produto?.StatusAtivo == 1;
        }

        /// <summary>
        /// Obt�m status formatado
        /// </summary>
        public static string GetStatusFormatado(this ProdutoDto produto)
        {
            return produto?.EstaAtivo() == true ? "Ativo" : "Inativo";
        }

        /// <summary>
        /// Verifica se o produto est� com estoque baixo
        /// </summary>
        public static bool EstoqueAbaixoMinimo(this ProdutoDto produto, int estoqueMinimo = 10)
        {
            return produto?.QuantidadeEstoqueProduto <= estoqueMinimo;
        }

        /// <summary>
        /// Obt�m informa��es de alerta do produto
        /// </summary>
        public static List<string> GetAlertas(this ProdutoDto produto, int estoqueMinimo = 10, int diasVencimento = 30)
        {
            var alertas = new List<string>();

            if (produto == null) return alertas;

            // Alerta de estoque baixo
            if (produto.EstoqueAbaixoMinimo(estoqueMinimo))
            {
                alertas.Add($"Estoque baixo ({produto.QuantidadeEstoqueProduto} unidades)");
            }

            // Alerta de produto pr�ximo ao vencimento
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
            if (produto.MargemLucro < 10)
            {
                alertas.Add($"Margem baixa ({produto.MargemLucro:F1}%)");
            }

            // Alerta de pre�o de venda menor que custo
            if (produto.PrecoVenda <= produto.PrecoCusto)
            {
                alertas.Add("Pre�o de venda menor que custo");
            }

            return alertas;
        }

        /// <summary>
        /// Obt�m cor do alerta baseado na criticidade
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
            return produto.PrecoVenda - produto.PrecoCusto;
        }

        /// <summary>
        /// Calcula valor total do estoque
        /// </summary>
        public static decimal CalcularValorTotalEstoque(this ProdutoDto produto)
        {
            if (produto == null) return 0;
            return produto.PrecoCusto * produto.QuantidadeEstoqueProduto;
        }

        /// <summary>
        /// Obt�m informa��es completas de estoque formatadas
        /// </summary>
        public static string GetInfoEstoqueCompleta(this ProdutoDto produto)
        {
            if (produto == null) return string.Empty;

            var info = $"Qtd: {produto.QuantidadeEstoqueProduto} | ";
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
                   produto.QuantidadeEstoqueProduto > 0 && 
                   (!produto.EhPerecivel || produto.DiasVencimento > 0);
        }

        /// <summary>
        /// Obt�m categoria de produto baseada no c�digo de barras
        /// </summary>
        public static string GetCategoriaPorCodigoBarras(this ProdutoDto produto)
        {
            if (produto?.CodBarras == null || produto.CodBarras.Length < 3)
                return "Geral";

            var prefixo = produto.CodBarras.Substring(0, 3);

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
        /// Gera c�digo de barras sugerido baseado na categoria
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
                _ => "789" // Nacional gen�rico
            };

            var timestamp = DateTimeOffset.Now.ToUnixTimeSeconds().ToString();
            var sufixo = timestamp.Substring(timestamp.Length - 9); // �ltimos 9 d�gitos

            var codigoSemDigito = prefixo + sufixo;
            var digitoVerificador = CalcularDigitoVerificadorEAN13(codigoSemDigito);

            return codigoSemDigito + digitoVerificador;
        }

        /// <summary>
        /// Calcula d�gito verificador para c�digo EAN-13
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
        /// Obt�m resumo do produto para relat�rios
        /// </summary>
        public static string GetResumo(this ProdutoDto produto)
        {
            if (produto == null) return string.Empty;

            return $"{produto.NomeProduto} | " +
                   $"C�d: {produto.CodBarras.FormatCodigoBarras()} | " +
                   $"Pre�o: {produto.PrecoVenda.FormatPreco()} | " +
                   $"Estoque: {produto.QuantidadeEstoqueProduto} | " +
                   $"Margem: {produto.MargemLucro:F1}%";
        }

        /// <summary>
        /// Verifica se � um produto sazonal baseado na data
        /// </summary>
        public static bool EhProdutoSazonal(this ProdutoDto produto)
        {
            if (produto?.NomeProduto == null) return false;

            var nome = produto.NomeProduto.ToLower();
            var mes = DateTime.Now.Month;

            // Produtos natalinos
            if ((mes == 11 || mes == 12) && (nome.Contains("natal") || nome.Contains("papai noel")))
                return true;

            // Produtos de p�scoa
            if ((mes == 3 || mes == 4) && (nome.Contains("p�scoa") || nome.Contains("chocolate") && nome.Contains("ovo")))
                return true;

            // Produtos de festa junina
            if ((mes == 6 || mes == 7) && (nome.Contains("junina") || nome.Contains("milho") || nome.Contains("festa")))
                return true;

            return false;
        }
    }
}