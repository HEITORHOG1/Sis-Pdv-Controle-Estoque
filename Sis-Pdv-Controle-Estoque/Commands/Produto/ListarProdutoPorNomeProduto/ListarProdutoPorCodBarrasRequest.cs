using MediatR;

namespace Commands.Produto.ListarProdutoPorNomeProduto
{
    public class ListarProdutoPorCodBarrasRequest : IRequest<Commands.Response>
    {
        public ListarProdutoPorCodBarrasRequest(string CodBarras)
        {
            this.CodBarras = CodBarras;
        }
        public string CodBarras { get; set; }
    }
}

