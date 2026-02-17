using MediatR;

namespace Commands.Departamento.ListarDepartamentoPorNomeDepartamento
{
    public class ListarDepartamentoPorNomeDepartamentoRequest : IRequest<ListarDepartamentoPorNomeDepartamentoResponse>
    {
        public ListarDepartamentoPorNomeDepartamentoRequest(string NomeDepartamento)
        {
            this.NomeDepartamento = NomeDepartamento;
        }
        public string NomeDepartamento { get; set; }
    }
}

