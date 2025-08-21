using Sis_Pdv_Controle_Estoque_Form.Dto.Movimentacao;
using Sis_Pdv_Controle_Estoque_Form.IServices;
using Sis_Pdv_Controle_Estoque_Form.Services.Base;
using Sis_Pdv_Controle_Estoque_Form.Utils;

namespace Sis_Pdv_Controle_Estoque_Form.Services.Inventory
{
    /// <summary>
    /// Serviço completo para operações de inventário e estoque
    /// </summary>
    public class InventoryService : BaseApiService, IInventoryService
    {
        public InventoryService() : base(BaseAppConfig.ReadSetting("Base"))
        {
        }

        /// <summary>
        /// Atualiza o estoque de um produto
        /// </summary>
        public async Task<MovimentacaoEstoqueDto> AtualizarEstoque(string produtoId, decimal quantidade)
        {
            ValidateGuid(produtoId, nameof(produtoId));
            
            if (quantidade < 0)
                throw new ArgumentException("Quantidade não pode ser negativa.", nameof(quantidade));

            var request = new
            {
                ProdutoId = produtoId,
                QuantidadeEstoque = quantidade,
                Tipo = (int)TipoMovimentacao.Ajuste,
                Motivo = "Atualização direta de estoque"
            };

            return await ExecuteWithRetry<MovimentacaoEstoqueDto>(
                () => _client.PutAsJson($"{_basePath}/Inventory/AtualizarEstoque", request),
                nameof(AtualizarEstoque)
            );
        }

        /// <summary>
        /// Cria uma movimentação de estoque
        /// </summary>
        public async Task<MovimentacaoEstoqueDto> CriarMovimentacao(CriarMovimentacaoDto movimentacao)
        {
            if (movimentacao == null)
                throw new ArgumentNullException(nameof(movimentacao));

            ValidateGuid(movimentacao.ProdutoId.ToString(), nameof(movimentacao.ProdutoId));
            ValidateRequired(movimentacao.Motivo, nameof(movimentacao.Motivo));

            if (movimentacao.Quantidade <= 0)
                throw new ArgumentException("Quantidade deve ser maior que zero.", nameof(movimentacao.Quantidade));

            return await ExecuteWithRetry<MovimentacaoEstoqueDto>(
                () => _client.PostAsJson($"{_basePath}/Inventory/CriarMovimentacao", movimentacao),
                nameof(CriarMovimentacao)
            );
        }

        /// <summary>
        /// Lista movimentações de estoque com filtros
        /// </summary>
        public async Task<MovimentacoesPaginadasDto> ListarMovimentacoes(FiltroMovimentacaoDto filtro)
        {
            var queryParams = new List<string>();

            if (filtro.ProdutoId.HasValue)
            {
                ValidateGuid(filtro.ProdutoId.Value.ToString(), nameof(filtro.ProdutoId));
                queryParams.Add($"produtoId={filtro.ProdutoId.Value}");
            }

            if (filtro.Tipo.HasValue)
                queryParams.Add($"tipo={filtro.Tipo.Value}");

            if (filtro.DataInicio.HasValue)
                queryParams.Add($"dataInicio={filtro.DataInicio.Value:yyyy-MM-dd}");

            if (filtro.DataFim.HasValue)
                queryParams.Add($"dataFim={filtro.DataFim.Value:yyyy-MM-dd}");

            if (!string.IsNullOrWhiteSpace(filtro.NomeProduto))
                queryParams.Add($"nomeProduto={Uri.EscapeDataString(filtro.NomeProduto)}");

            if (!string.IsNullOrWhiteSpace(filtro.CodigoBarras))
                queryParams.Add($"codigoBarras={Uri.EscapeDataString(filtro.CodigoBarras)}");

            if (!string.IsNullOrWhiteSpace(filtro.Lote))
                queryParams.Add($"lote={Uri.EscapeDataString(filtro.Lote)}");

            queryParams.Add($"pagina={filtro.Pagina}");
            queryParams.Add($"tamanhoPagina={filtro.TamanhoPagina}");

            var query = queryParams.Count > 0 ? "?" + string.Join("&", queryParams) : "";

            return await ExecuteWithRetry<MovimentacoesPaginadasDto>(
                () => _client.GetAsync($"{_basePath}/Inventory/ListarMovimentacoes{query}"),
                nameof(ListarMovimentacoes)
            );
        }

        /// <summary>
        /// Consulta o estoque atual de um produto
        /// </summary>
        public async Task<ValidacaoEstoqueDto> ConsultarEstoque(string produtoId)
        {
            ValidateGuid(produtoId, nameof(produtoId));

            return await ExecuteWithRetry<ValidacaoEstoqueDto>(
                () => _client.GetAsync($"{_basePath}/Inventory/ConsultarEstoque/{produtoId}"),
                nameof(ConsultarEstoque)
            );
        }

        /// <summary>
        /// Valida se há estoque suficiente para uma operação
        /// </summary>
        public async Task<ValidacaoEstoqueDto> ValidarEstoque(string produtoId, decimal quantidadeRequerida)
        {
            ValidateGuid(produtoId, nameof(produtoId));
            
            if (quantidadeRequerida <= 0)
                throw new ArgumentException("Quantidade requerida deve ser maior que zero.", nameof(quantidadeRequerida));

            return await ExecuteWithRetry<ValidacaoEstoqueDto>(
                () => _client.GetAsync($"{_basePath}/Inventory/ValidarEstoque/{produtoId}?quantidade={quantidadeRequerida}"),
                nameof(ValidarEstoque)
            );
        }

        /// <summary>
        /// Lista produtos com estoque baixo
        /// </summary>
        public async Task<List<AlertaEstoqueDto>> ListarProdutosEstoqueBaixo(decimal? limiteMinimo = null)
        {
            var query = limiteMinimo.HasValue ? $"?limite={limiteMinimo.Value}" : "";

            return await ExecuteWithRetry<List<AlertaEstoqueDto>>(
                () => _client.GetAsync($"{_basePath}/Inventory/EstoqueBaixo{query}"),
                nameof(ListarProdutosEstoqueBaixo)
            );
        }

        /// <summary>
        /// Executa uma entrada de estoque
        /// </summary>
        public async Task<MovimentacaoEstoqueDto> EntradaEstoque(string produtoId, decimal quantidade, string motivo, string? lote = null, DateTime? dataValidade = null)
        {
            ValidateGuid(produtoId, nameof(produtoId));
            ValidateRequired(motivo, nameof(motivo));
            
            if (quantidade <= 0)
                throw new ArgumentException("Quantidade deve ser maior que zero.", nameof(quantidade));

            var movimentacao = new CriarMovimentacaoDto
            {
                ProdutoId = Guid.Parse(produtoId),
                Tipo = (int)TipoMovimentacao.Entrada,
                Quantidade = quantidade,
                Motivo = motivo,
                Lote = lote,
                DataValidade = dataValidade
            };

            return await CriarMovimentacao(movimentacao);
        }

        /// <summary>
        /// Executa uma saída de estoque
        /// </summary>
        public async Task<MovimentacaoEstoqueDto> SaidaEstoque(string produtoId, decimal quantidade, string motivo, string? lote = null)
        {
            ValidateGuid(produtoId, nameof(produtoId));
            ValidateRequired(motivo, nameof(motivo));
            
            if (quantidade <= 0)
                throw new ArgumentException("Quantidade deve ser maior que zero.", nameof(quantidade));

            // Valida se há estoque suficiente antes da saída
            var validacao = await ValidarEstoque(produtoId, quantidade);
            if (!validacao.EstaDisponivel)
            {
                throw new InvalidOperationException($"Estoque insuficiente. Disponível: {validacao.QuantidadeDisponivel}, Solicitado: {quantidade}");
            }

            var movimentacao = new CriarMovimentacaoDto
            {
                ProdutoId = Guid.Parse(produtoId),
                Tipo = (int)TipoMovimentacao.Saida,
                Quantidade = quantidade,
                Motivo = motivo,
                Lote = lote
            };

            return await CriarMovimentacao(movimentacao);
        }

        /// <summary>
        /// Executa ajuste de estoque (correção)
        /// </summary>
        public async Task<MovimentacaoEstoqueDto> AjusteEstoque(string produtoId, decimal quantidadeAtual, decimal quantidadeCorreta, string motivo)
        {
            ValidateGuid(produtoId, nameof(produtoId));
            ValidateRequired(motivo, nameof(motivo));
            
            if (quantidadeAtual < 0)
                throw new ArgumentException("Quantidade atual não pode ser negativa.", nameof(quantidadeAtual));
            
            if (quantidadeCorreta < 0)
                throw new ArgumentException("Quantidade correta não pode ser negativa.", nameof(quantidadeCorreta));

            if (quantidadeAtual == quantidadeCorreta)
                throw new ArgumentException("Quantidade atual e correta são iguais. Não há necessidade de ajuste.");

            var quantidadeMovimentacao = Math.Abs(quantidadeCorreta - quantidadeAtual);
            var motivoCompleto = $"{motivo}. Ajuste: {quantidadeAtual} → {quantidadeCorreta}";

            var movimentacao = new CriarMovimentacaoDto
            {
                ProdutoId = Guid.Parse(produtoId),
                Tipo = (int)TipoMovimentacao.Ajuste,
                Quantidade = quantidadeMovimentacao,
                Motivo = motivoCompleto
            };

            return await CriarMovimentacao(movimentacao);
        }

        protected override string GetNotFoundMessage(string methodName)
        {
            return methodName switch
            {
                nameof(AtualizarEstoque) => "Produto não encontrado para atualização de estoque",
                nameof(CriarMovimentacao) => "Produto não encontrado para criar movimentação",
                nameof(ConsultarEstoque) => "Produto não encontrado para consulta de estoque",
                nameof(ValidarEstoque) => "Produto não encontrado para validação de estoque",
                nameof(EntradaEstoque) => "Produto não encontrado para entrada de estoque",
                nameof(SaidaEstoque) => "Produto não encontrado para saída de estoque",
                nameof(AjusteEstoque) => "Produto não encontrado para ajuste de estoque",
                _ => "Recurso não encontrado"
            };
        }

        protected override string GetConflictMessage(string methodName)
        {
            return methodName switch
            {
                nameof(CriarMovimentacao) => "Conflito ao criar movimentação - movimentação duplicada",
                nameof(SaidaEstoque) => "Conflito na saída de estoque - estoque insuficiente",
                _ => "Conflito de dados"
            };
        }
    }
}
