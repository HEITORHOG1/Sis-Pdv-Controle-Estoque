using Commands.Pedidos.ListarVendaPedidoPorData;
using Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Repositories
{
    public class RepositoryPedido : RepositoryBase<Pedido, Guid>, IRepositoryPedido
    {
        public RepositoryPedido(PdvContext context) : base(context)
        {
        }

        public async Task<IList<ListarVendaPedidoPorDataResponse>> ListarVendaPedidoPorData(DateTime DataInicio, DateTime DataFim)
        {
            return await _context.Set<Pedido>()
                .Where(p => !p.IsDeleted && 
                           p.DataDoPedido.HasValue &&
                           p.DataDoPedido.Value.Date >= DataInicio.Date && 
                           p.DataDoPedido.Value.Date <= DataFim.Date)
                .Select(p => new ListarVendaPedidoPorDataResponse
                {
                    Id = p.Id,
                    DataDoPedido = p.DataDoPedido,
                    FormaPagamento = p.FormaPagamento,
                    TotalPedido = p.TotalPedido
                })
                .ToListAsync();
        }
    }
}
