using Sis_Pdv_Controle_Estoque_Form.Dto.PDV;
using Sis_Pdv_Controle_Estoque_Form.Dto.Produto;

namespace Sis_Pdv_Controle_Estoque_Form.Extensions
{
    /// <summary>
    /// Extens�es espec�ficas para opera��es do PDV
    /// </summary>
    public static class PdvExtensions
    {
        /// <summary>
        /// Converte ProdutoDto para ItemCarrinhoDto
        /// </summary>
        public static ItemCarrinhoDto ToItemCarrinho(this Data produto, int quantidade = 1)
        {
            return new ItemCarrinhoDto
            {
                CodigoBarras = produto.CodBarras ?? string.Empty,
                Descricao = produto.NomeProduto ?? string.Empty,
                PrecoUnitario = produto.PrecoVenda,
                Quantidade = quantidade,
                ProdutoId = produto.Id,
                EstoqueDisponivel = produto.QuantidadeEstoqueProduto,
                DataVencimento = produto.DataVencimento > DateTime.MinValue ? produto.DataVencimento : null
            };
        }

        /// <summary>
        /// Verifica se produto pode ser vendido
        /// </summary>
        public static bool PodeSerVendido(this Data produto)
        {
            // Produto ativo
            if (produto.StatusAtivo != 1) return false;
            
            // Tem estoque dispon�vel
            if (produto.QuantidadeEstoqueProduto <= 0) return false;
            
            // N�o est� vencido
            if (produto.DataVencimento > DateTime.MinValue && produto.DataVencimento <= DateTime.Now)
                return false;
            
            return true;
        }

        /// <summary>
        /// Obt�m alertas do produto para venda
        /// </summary>
        public static List<string> GetAlertasVenda(this Data produto)
        {
            var alertas = new List<string>();
            
            if (produto.StatusAtivo != 1)
                alertas.Add("Produto inativo");
            
            if (produto.QuantidadeEstoqueProduto <= 0)
                alertas.Add("Produto sem estoque");
            else if (produto.QuantidadeEstoqueProduto <= 10)
                alertas.Add($"Estoque baixo ({produto.QuantidadeEstoqueProduto} unidades)");
            
            if (produto.DataVencimento > DateTime.MinValue)
            {
                var diasVencimento = (produto.DataVencimento - DateTime.Now).Days;
                if (diasVencimento <= 0)
                    alertas.Add("Produto vencido");
                else if (diasVencimento <= 7)
                    alertas.Add($"Vence em {diasVencimento} dias");
            }
            
            return alertas;
        }

        /// <summary>
        /// Obt�m cor do alerta para exibi��o no PDV
        /// </summary>
        public static System.Drawing.Color GetCorAlertaPdv(this Data produto)
        {
            var alertas = produto.GetAlertasVenda();
            
            if (alertas.Any(a => a.Contains("vencido") || a.Contains("inativo") || a.Contains("sem estoque")))
                return System.Drawing.Color.Red;
            
            if (alertas.Any(a => a.Contains("Vence em") || a.Contains("baixo")))
                return System.Drawing.Color.Orange;
            
            return System.Drawing.Color.Green;
        }

        /// <summary>
        /// Formata valor monet�rio para exibi��o
        /// </summary>
        public static string FormatarMoeda(this decimal valor)
        {
            return valor.ToString("C2", System.Globalization.CultureInfo.GetCultureInfo("pt-BR"));
        }

        /// <summary>
        /// Formata quantidade para exibi��o
        /// </summary>
        public static string FormatarQuantidade(this int quantidade)
        {
            return quantidade.ToString("N0");
        }

        /// <summary>
        /// Valida CPF/CNPJ para cliente
        /// </summary>
        public static bool IsValidCpfCnpj(this string CpfCnpj)
        {
            if (string.IsNullOrWhiteSpace(CpfCnpj))
                return false;
            
            var numbersOnly = new string(CpfCnpj.Where(char.IsDigit).ToArray());
            
            return numbersOnly.Length == 11 ? IsValidCpf(numbersOnly) : 
                   numbersOnly.Length == 14 ? IsValidCnpj(numbersOnly) : false;
        }

        /// <summary>
        /// Valida CPF
        /// </summary>
        private static bool IsValidCpf(string cpf)
        {
            if (cpf.Length != 11 || cpf.All(c => c == cpf[0]))
                return false;

            var sum = 0;
            for (int i = 0; i < 9; i++)
                sum += int.Parse(cpf[i].ToString()) * (10 - i);

            var remainder = sum % 11;
            var digit1 = remainder < 2 ? 0 : 11 - remainder;

            if (int.Parse(cpf[9].ToString()) != digit1)
                return false;

            sum = 0;
            for (int i = 0; i < 10; i++)
                sum += int.Parse(cpf[i].ToString()) * (11 - i);

            remainder = sum % 11;
            var digit2 = remainder < 2 ? 0 : 11 - remainder;

            return int.Parse(cpf[10].ToString()) == digit2;
        }

        /// <summary>
        /// Valida CNPJ
        /// </summary>
        private static bool IsValidCnpj(string cnpj)
        {
            if (cnpj.Length != 14 || cnpj.All(c => c == cnpj[0]))
                return false;

            var weights1 = new int[] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            var weights2 = new int[] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

            var sum = 0;
            for (int i = 0; i < 12; i++)
                sum += int.Parse(cnpj[i].ToString()) * weights1[i];

            var remainder = sum % 11;
            var digit1 = remainder < 2 ? 0 : 11 - remainder;

            if (int.Parse(cnpj[12].ToString()) != digit1)
                return false;

            sum = 0;
            for (int i = 0; i < 13; i++)
                sum += int.Parse(cnpj[i].ToString()) * weights2[i];

            remainder = sum % 11;
            var digit2 = remainder < 2 ? 0 : 11 - remainder;

            return int.Parse(cnpj[13].ToString()) == digit2;
        }

        /// <summary>
        /// Calcula percentual de desconto
        /// </summary>
        public static decimal CalcularPercentualDesconto(this decimal valorOriginal, decimal valorDesconto)
        {
            if (valorOriginal == 0) return 0;
            return (valorDesconto / valorOriginal) * 100;
        }

        /// <summary>
        /// Calcula valor do desconto por percentual
        /// </summary>
        public static decimal CalcularValorDesconto(this decimal valorOriginal, decimal percentual)
        {
            return valorOriginal * (percentual / 100);
        }

        /// <summary>
        /// Verifica se � forma de pagamento � vista
        /// </summary>
        public static bool IsFormaPagamentoVista(this string FormaPagamento)
        {
            var formasVista = new[] { "DINHEIRO", "PIX", "DEBITO" };
            return formasVista.Any(f => f.Equals(FormaPagamento?.ToUpper()));
        }

        /// <summary>
        /// Verifica se � forma de pagamento parcelada
        /// </summary>
        public static bool IsFormaPagamentoParcelada(this string FormaPagamento)
        {
            return FormaPagamento?.ToUpper().Contains("CREDITO") == true ||
                   FormaPagamento?.ToUpper().Contains("PARCEL") == true;
        }

        /// <summary>
        /// Gera n�mero de controle da venda
        /// </summary>
        public static string GerarNumeroControle(this VendaDto venda)
        {
            var timestamp = DateTimeOffset.Now.ToUnixTimeSeconds();
            var hash = venda.ColaboradorId.ToString("N")[..8];
            return $"VD{timestamp:X}{hash}".ToUpper();
        }

        /// <summary>
        /// Valida se venda pode ser finalizada
        /// </summary>
        public static bool PodeSerFinalizada(this VendaDto venda)
        {
            return venda.Itens.Any(i => !i.Cancelado) &&
                   venda.ColaboradorId != Guid.Empty &&
                   !string.IsNullOrWhiteSpace(venda.FormaPagamento) &&
                   venda.ValorRecebido >= venda.ValorFinal;
        }

        /// <summary>
        /// Obt�m texto do status da venda
        /// </summary>
        public static string GetStatusTexto(this VendaDto venda)
        {
            return venda.StatusVenda switch
            {
                "ABERTA" => "?? Venda em Andamento",
                "FINALIZADA" => "?? Venda Finalizada",
                "CANCELADA" => "?? Venda Cancelada",
                _ => "? Status Desconhecido"
            };
        }

        /// <summary>
        /// Obt�m resumo dos itens para cupom
        /// </summary>
        public static string GetResumoItens(this VendaDto venda)
        {
            var itensAtivos = venda.Itens.Where(i => !i.Cancelado).ToList();
            var itensCancelados = venda.Itens.Where(i => i.Cancelado).ToList();
            
            var resumo = $"Itens: {itensAtivos.Count}";
            if (itensCancelados.Any())
                resumo += $" (Cancelados: {itensCancelados.Count})";
            
            return resumo;
        }

        /// <summary>
        /// Converte tempo para formato amig�vel
        /// </summary>
        public static string ToFriendlyTime(this TimeSpan tempo)
        {
            if (tempo.TotalSeconds < 60)
                return $"{tempo.Seconds}s";
            if (tempo.TotalMinutes < 60)
                return $"{tempo.Minutes}m {tempo.Seconds}s";
            return $"{tempo.Hours}h {tempo.Minutes}m";
        }

        /// <summary>
        /// Obt�m c�digo de cores para DataGridView
        /// </summary>
        public static System.Drawing.Color GetCorStatus(this ItemCarrinhoDto item)
        {
            if (item.Cancelado)
                return System.Drawing.Color.LightCoral;
            
            var alertas = item.Validar();
            if (alertas.Any(a => a.Contains("vencido") || a.Contains("estoque")))
                return System.Drawing.Color.Orange;
            
            return System.Drawing.Color.LightGreen;
        }

        /// <summary>
        /// Trunca texto para exibi��o em grid
        /// </summary>
        public static string TruncateForDisplay(this string texto, int maxLength = 30)
        {
            if (string.IsNullOrEmpty(texto) || texto.Length <= maxLength)
                return texto ?? string.Empty;
            
            return texto.Substring(0, maxLength - 3) + "...";
        }

        /// <summary>
        /// Formata data/hora para cupom fiscal
        /// </summary>
        public static string FormatarDataHoraCupom(this DateTime dateTime)
        {
            return dateTime.ToString("dd/MM/yyyy HH:mm:ss");
        }

        /// <summary>
        /// Valida se opera��o PDV � v�lida
        /// </summary>
        public static bool ValidarOperacaoPdv(this string operacao, List<string> operacoesPermitidas)
        {
            return operacoesPermitidas.Contains(operacao?.ToUpper());
        }

        /// <summary>
        /// Obt�m �cone para forma de pagamento
        /// </summary>
        public static string GetIconeFormaPagamento(this string FormaPagamento)
        {
            return FormaPagamento?.ToUpper() switch
            {
                "DINHEIRO" => "??",
                "CARTAO" or "CREDITO" or "DEBITO" => "??",
                "PIX" => "??",
                "CHEQUE" => "??",
                _ => "??"
            };
        }
    }
}