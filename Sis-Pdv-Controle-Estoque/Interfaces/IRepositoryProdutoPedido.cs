using Commands.Pedido.ListarVendaPedidoPorData;
using Commands.ProdutoPedido.ListarProdutoPedidoPorId;
using Sis_Pdv_Controle_Estoque.Interfaces.Repositories.Base;
using Sis_Pdv_Controle_Estoque.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sis_Pdv_Controle_Estoque.Interfaces
{
    public interface IRepositoryProdutoPedido : IRepositoryBase<ProdutoPedido, Guid>
    {
        Task<IList<ListarProdutosPorPedidoIdResponse>> ListarProdutosPorPedidoId(Guid Id);
    }
}
