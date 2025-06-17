using MediatR;

namespace Commands.Fornecedor.ListarFornecedorPorNomeDepartamento
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

