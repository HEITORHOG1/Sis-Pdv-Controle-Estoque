using Sis_Pdv_Controle_Estoque_Form.Dto.Produto;

namespace Sis_Pdv_Controle_Estoque_Form.IServices
{
    /// <summary>
    /// Interface para operações de dados mestres de produtos
    /// (Não inclui operações de estoque - use IInventoryService para isso)
    /// </summary>
    public interface IProdutoService
    {
        /// <summary>
        /// Adiciona um novo produto ao sistema
        /// </summary>
        /// <param name="dto">Dados do produto</param>
        /// <returns>Resposta da operação</returns>
        Task<ProdutoResponse> AdicionarProduto(ProdutoDto dto);

        /// <summary>
        /// Lista todos os produtos
        /// </summary>
        /// <returns>Lista de produtos</returns>
        Task<ProdutoResponseList> ListarProduto();

        /// <summary>
        /// Busca produto por ID
        /// </summary>
        /// <param name="id">ID do produto</param>
        /// <returns>Dados do produto</returns>
        Task<ProdutoResponseList> ListarProdutoPorId(string id);

        /// <summary>
        /// Busca produto por código de barras
        /// </summary>
        /// <param name="codBarras">Código de barras</param>
        /// <returns>Dados do produto</returns>
        Task<ProdutoResponseList> ListarProdutoPorCodBarras(string codBarras);

        /// <summary>
        /// Altera dados de um produto existente
        /// </summary>
        /// <param name="dto">Dados atualizados do produto</param>
        /// <returns>Resposta da operação</returns>
        Task<ProdutoResponse> AlterarProduto(ProdutoDto dto);

        /// <summary>
        /// Remove um produto do sistema
        /// </summary>
        /// <param name="id">ID do produto</param>
        /// <returns>Resposta da operação</returns>
        Task<ProdutoResponse> RemoverProduto(string id);
    }
}
