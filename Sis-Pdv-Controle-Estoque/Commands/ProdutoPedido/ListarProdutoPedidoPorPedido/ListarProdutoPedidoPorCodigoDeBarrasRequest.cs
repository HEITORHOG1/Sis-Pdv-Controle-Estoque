using MediatR;

namespace Commands.ProdutoPedido.ListarProdutoPedidoPorPedido
{
    public class ListarProdutoPedidoPorCodigoDeBarrasRequest : IRequest<Sis_Pdv_Controle_Estoque.Commands.Response>
    {
        public ListarProdutoPedidoPorCodigoDeBarrasRequest(string codBarras)
        {
            CodBarras = codBarras;
        }

        public string CodBarras { get; set; }
    }
}

