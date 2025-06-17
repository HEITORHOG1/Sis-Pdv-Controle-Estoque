using MediatR;

namespace Commands.Departamento.AlterarDepartamento
{
    public class AlterarDepartamentoRequest : IRequest<Sis_Pdv_Controle_Estoque.Commands.Response>
    {
        public Guid Id { get; set; }
        public string NomeDepartamento { get; set; }
    }
}
