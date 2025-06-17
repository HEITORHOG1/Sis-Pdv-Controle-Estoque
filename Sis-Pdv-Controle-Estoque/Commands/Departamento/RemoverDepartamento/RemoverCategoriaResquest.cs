using MediatR;

namespace Commands.Departamento.RemoverDepartamento
{
    public class RemoverDepartamentoResquest : IRequest<Response>
    {
        public RemoverDepartamentoResquest(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}
