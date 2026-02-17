using MediatR;

namespace Commands.Fornecedor.ListarFornecedorPorNomeFornecedor
{
    public class ListarFornecedorPorNomeFornecedorRequest : IRequest<ListarFornecedorPorNomeFornecedorResponse>
    {
        public ListarFornecedorPorNomeFornecedorRequest(string Cnpj)
        {
            this.Cnpj = Cnpj;
        }
        public string Cnpj { get; set; }
    }
}

