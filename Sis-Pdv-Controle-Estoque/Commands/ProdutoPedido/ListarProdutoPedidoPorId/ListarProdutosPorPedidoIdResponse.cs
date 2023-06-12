using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commands.ProdutoPedido.ListarProdutoPedidoPorId
{
    public  class ListarProdutosPorPedidoIdResponse
    {
        public int quantidadeItemPedido { get; set; }
        public string nomeProduto { get; set; }
        public decimal precoVenda { get; set; }
        public decimal totalProdutoPedido { get; set; }
    }
}
