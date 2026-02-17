using MediatR;

namespace Commands.Fornecedor.RemoverFornecedor
{
    public class RemoverFornecedorRequest : IRequest<Response>
    {
        public RemoverFornecedorRequest(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}
