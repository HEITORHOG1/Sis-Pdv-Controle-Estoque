using Sis_Pdv_Controle_Estoque_Form.Dto.Movimentacao;

namespace Sis_Pdv_Controle_Estoque_Form.IServices
{
    /// <summary>
    /// Interface para operações de inventário e estoque
    /// </summary>
    public interface IInventoryService
    {
        /// <summary>
        /// Atualiza o estoque de um produto
        /// </summary>
        /// <param name="produtoId">ID do produto</param>
        /// <param name="quantidade">Nova quantidade em estoque</param>
        /// <returns>Resposta da operação</returns>
        Task<MovimentacaoEstoqueDto> AtualizarEstoque(string produtoId, decimal quantidade);

        /// <summary>
        /// Cria uma movimentação de estoque
        /// </summary>
        /// <param name="movimentacao">Dados da movimentação</param>
        /// <returns>Resposta da operação</returns>
        Task<MovimentacaoEstoqueDto> CriarMovimentacao(CriarMovimentacaoDto movimentacao);

        /// <summary>
        /// Lista movimentações de estoque com filtros
        /// </summary>
        /// <param name="filtro">Filtros para a busca</param>
        /// <returns>Lista paginada de movimentações</returns>
        Task<MovimentacoesPaginadasDto> ListarMovimentacoes(FiltroMovimentacaoDto filtro);

        /// <summary>
        /// Consulta o estoque atual de um produto
        /// </summary>
        /// <param name="produtoId">ID do produto</param>
        /// <returns>Informações do estoque</returns>
        Task<ValidacaoEstoqueDto> ConsultarEstoque(string produtoId);

        /// <summary>
        /// Valida se há estoque suficiente para uma operação
        /// </summary>
        /// <param name="produtoId">ID do produto</param>
        /// <param name="quantidadeRequerida">Quantidade necessária</param>
        /// <returns>Resultado da validação</returns>
        Task<ValidacaoEstoqueDto> ValidarEstoque(string produtoId, decimal quantidadeRequerida);

        /// <summary>
        /// Lista produtos com estoque baixo
        /// </summary>
        /// <param name="limiteMinimo">Limite mínimo de estoque (opcional)</param>
        /// <returns>Lista de alertas de estoque</returns>
        Task<List<AlertaEstoqueDto>> ListarProdutosEstoqueBaixo(decimal? limiteMinimo = null);

        /// <summary>
        /// Executa uma entrada de estoque
        /// </summary>
        /// <param name="produtoId">ID do produto</param>
        /// <param name="quantidade">Quantidade a adicionar</param>
        /// <param name="motivo">Motivo da entrada</param>
        /// <param name="lote">Lote do produto (opcional)</param>
        /// <param name="dataValidade">Data de validade (opcional)</param>
        /// <returns>Resultado da operação</returns>
        Task<MovimentacaoEstoqueDto> EntradaEstoque(string produtoId, decimal quantidade, string motivo, string? lote = null, DateTime? dataValidade = null);

        /// <summary>
        /// Executa uma saída de estoque
        /// </summary>
        /// <param name="produtoId">ID do produto</param>
        /// <param name="quantidade">Quantidade a retirar</param>
        /// <param name="motivo">Motivo da saída</param>
        /// <param name="lote">Lote específico (opcional)</param>
        /// <returns>Resultado da operação</returns>
        Task<MovimentacaoEstoqueDto> SaidaEstoque(string produtoId, decimal quantidade, string motivo, string? lote = null);

        /// <summary>
        /// Executa ajuste de estoque (correção)
        /// </summary>
        /// <param name="produtoId">ID do produto</param>
        /// <param name="quantidadeAtual">Quantidade atual no sistema</param>
        /// <param name="quantidadeCorreta">Quantidade correta a ser definida</param>
        /// <param name="motivo">Motivo do ajuste</param>
        /// <returns>Resultado da operação</returns>
        Task<MovimentacaoEstoqueDto> AjusteEstoque(string produtoId, decimal quantidadeAtual, decimal quantidadeCorreta, string motivo);
    }
}
