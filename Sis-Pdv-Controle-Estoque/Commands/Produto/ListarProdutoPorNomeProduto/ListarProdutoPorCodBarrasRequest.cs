using MediatR;

namespace Commands.Produto.ListarProdutoPorNomeProduto
{
    public class ListarProdutoPorCodBarrasRequest : IRequest<Sis_Pdv_Controle_Estoque.Commands.Response>
    {
        public ListarProdutoPorCodBarrasRequest(string codBarras)
        {
            this.codBarras = codBarras;
        }
        public string codBarras { get; set; }
    }
}

