using MediatR;

namespace Commands.Fornecedor.RemoverFornecedor
{
    public class RemoverFornecedorResquest : IRequest<Response>
    {
        public RemoverFornecedorResquest(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}
