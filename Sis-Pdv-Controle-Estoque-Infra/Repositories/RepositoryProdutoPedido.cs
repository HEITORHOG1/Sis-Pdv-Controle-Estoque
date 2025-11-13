using Commands.ProdutoPedido.ListarProdutoPedidoPorId;
using Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Repositories
{
    public class RepositoryProdutoPedido : RepositoryBase<ProdutoPedido, Guid>, IRepositoryProdutoPedido
    {
        private readonly PdvContext _context;
        public RepositoryProdutoPedido(PdvContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IList<ListarProdutosPorPedidoIdResponse>> ListarProdutosPorPedidoId(Guid Id)
        {
            return await _context.Set<ProdutoPedido>()
                .Where(pp => !pp.IsDeleted && pp.PedidoId == Id)
                .Include(pp => pp.Produto)
                .Select(pp => new ListarProdutosPorPedidoIdResponse
                {
                    quantidadeItemPedido = pp.QuantidadeItemPedido ?? 0,
                    nomeProduto = pp.Produto != null ? pp.Produto.NomeProduto : string.Empty,
                    precoVenda = pp.Produto != null ? pp.Produto.PrecoVenda : 0,
                    totalProdutoPedido = pp.TotalProdutoPedido ?? 0
                })
                .ToListAsync();
        }
    }
}
