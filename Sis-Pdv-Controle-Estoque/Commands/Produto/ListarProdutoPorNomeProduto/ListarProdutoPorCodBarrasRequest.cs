using MediatR;

namespace Commands.Produto.ListarProdutoPorNomeProduto
{
    public class ListarProdutoPorCodBarrasRequest : IRequest<Commands.Response>
    {
        public ListarProdutoPorCodBarrasRequest(string codBarras)
        {
            this.codBarras = codBarras;
        }
        public string codBarras { get; set; }
    }
}

