using MediatR;

namespace Commands.ProdutoPedido.ListarProdutoPedidoPorPedido
{
    public class ListarProdutoPedidoPorCodigoDeBarrasRequest : IRequest<Commands.Response>
    {
        public ListarProdutoPedidoPorCodigoDeBarrasRequest(string CodBarras)
        {
            CodBarras = CodBarras;
        }

        public string CodBarras { get; set; }
    }
}

