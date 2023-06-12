using MediatR;
using Sis_Pdv_Controle_Estoque.Commands;

namespace Commands.Departamento.AdicionarDepartamento
{
    public class AdicionarDepartamentoRequest : IRequest<Response>
    {
        public string NomeDepartamento { get; set; }
    }
}
