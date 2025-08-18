using Commands.Pedidos.ListarVendaPedidoPorData;
using Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Repositories
{
    public class RepositoryPedido : RepositoryBase<Pedido, Guid>, IRepositoryPedido
    {
        private readonly PdvContext _context;
        public RepositoryPedido(PdvContext context) : base(context)
        {
            _context = context;
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
                    dataDoPedido = p.DataDoPedido,
                    formaPagamento = p.FormaPagamento,
                    totalPedido = p.TotalPedido
                })
                .ToListAsync();
        }
    }
}
