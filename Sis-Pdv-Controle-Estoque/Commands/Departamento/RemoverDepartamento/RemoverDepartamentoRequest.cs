using MediatR;

namespace Commands.Departamento.RemoverDepartamento
{
    public class RemoverDepartamentoRequest : IRequest<Response>
    {
        public RemoverDepartamentoRequest(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}
