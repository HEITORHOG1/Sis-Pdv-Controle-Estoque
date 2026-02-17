using Commands.ProdutoPedido.ListarProdutoPedidoPorId;
using Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Repositories
{
    public class RepositoryProdutoPedido : RepositoryBase<ProdutoPedido, Guid>, IRepositoryProdutoPedido
    {
        public RepositoryProdutoPedido(PdvContext context) : base(context)
        {
        }

        public async Task<IList<ListarProdutosPorPedidoIdResponse>> ListarProdutosPorPedidoId(Guid Id)
        {
            return await _context.Set<ProdutoPedido>()
                .Where(pp => !pp.IsDeleted && pp.PedidoId == Id)
                .Include(pp => pp.Produto)
                .Select(pp => new ListarProdutosPorPedidoIdResponse
                {
                    QuantidadeItemPedido = pp.QuantidadeItemPedido ?? 0,
                    NomeProduto = pp.Produto != null ? pp.Produto.NomeProduto : string.Empty,
                    PrecoVenda = pp.Produto != null ? pp.Produto.PrecoVenda : 0,
                    TotalProdutoPedido = pp.TotalProdutoPedido ?? 0
                })
                .ToListAsync();
        }
    }
}
