using MediatR;

namespace Commands.Departamento.AlterarDepartamento
{
    public class AlterarDepartamentoRequest : IRequest<Commands.Response>
    {
        public Guid Id { get; set; }
        public string NomeDepartamento { get; set; }
    }
}
