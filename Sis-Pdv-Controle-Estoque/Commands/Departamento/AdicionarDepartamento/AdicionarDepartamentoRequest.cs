using MediatR;

namespace Commands.Departamento.AdicionarDepartamento
{
    public class AdicionarDepartamentoRequest : IRequest<Response>
    {
        public string NomeDepartamento { get; set; }
    }
}
