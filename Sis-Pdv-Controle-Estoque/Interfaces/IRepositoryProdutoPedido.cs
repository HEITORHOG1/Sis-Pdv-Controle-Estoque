using Commands.ProdutoPedido.ListarProdutoPedidoPorId;

namespace Interfaces
{
    public interface IRepositoryProdutoPedido : IRepositoryBase<ProdutoPedido, Guid>
    {
        Task<IList<ListarProdutosPorPedidoIdResponse>> ListarProdutosPorPedidoId(Guid Id);
    }
}
