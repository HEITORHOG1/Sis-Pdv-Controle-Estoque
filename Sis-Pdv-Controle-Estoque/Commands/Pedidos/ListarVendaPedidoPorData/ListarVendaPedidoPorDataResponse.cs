using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commands.Pedido.ListarVendaPedidoPorData
{
    public class ListarVendaPedidoPorDataResponse
    {
        public Guid? Id { get; set; }
        public DateTime? dataDoPedido { get; set; }
        public string formaPagamento { get; set; }
        public decimal totalPedido { get; set; }


    }
}
