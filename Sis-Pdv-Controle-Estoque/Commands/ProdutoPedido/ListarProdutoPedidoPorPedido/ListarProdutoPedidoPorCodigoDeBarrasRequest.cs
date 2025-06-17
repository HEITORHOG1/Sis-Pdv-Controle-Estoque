using MediatR;

namespace Commands.ProdutoPedido.ListarProdutoPedidoPorPedido
{
    public class ListarProdutoPedidoPorCodigoDeBarrasRequest : IRequest<Commands.Response>
    {
        public ListarProdutoPedidoPorCodigoDeBarrasRequest(string codBarras)
        {
            CodBarras = codBarras;
        }

        public string CodBarras { get; set; }
    }
}

