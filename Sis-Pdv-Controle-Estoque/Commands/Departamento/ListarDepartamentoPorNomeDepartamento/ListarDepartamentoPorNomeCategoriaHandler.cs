using MediatR;
using prmToolkit.NotificationPattern;
using Sis_Pdv_Controle_Estoque.Interfaces;
using System.Text.RegularExpressions;

namespace Commands.Departamento.ListarDepartamentoPorNomeDepartamento
{
    public class ListarDepartamentoPorNomeDepartamentoHandler : Notifiable, IRequestHandler<ListarDepartamentoPorNomeDepartamentoRequest, ListarDepartamentoPorNomeDepartamentoResponse>
    {
        private readonly IMediator _mediator;
        private readonly IRepositoryDepartamento _repositoryDepartamento;

        public ListarDepartamentoPorNomeDepartamentoHandler(IMediator mediator, IRepositoryDepartamento repositoryDepartamento)
        {
            _mediator = mediator;
            _repositoryDepartamento = repositoryDepartamento;
        }

        public async Task<ListarDepartamentoPorNomeDepartamentoResponse> Handle(ListarDepartamentoPorNomeDepartamentoRequest request, CancellationToken cancellationToken)
        {
            //Valida se o objeto request esta nulo
            if (request == null)
            {
                AddNotification("Erro", "Handle");
                return new ListarDepartamentoPorNomeDepartamentoResponse(this);
            }

            var Collection = _repositoryDepartamento.Listar().Where(x => x.NomeDepartamento == request.NomeDepartamento); 
            if (!Collection.Any()) 
            {
                AddNotification("ATENÇÃO", "Departamento NÃO ENCONTRADA");
                return new ListarDepartamentoPorNomeDepartamentoResponse(this);
            }

            //Criar meu objeto de resposta
            var response = new ListarDepartamentoPorNomeDepartamentoResponse(this, Collection);
            //Cria objeto de resposta

            ////Retorna o resultado
            return await Task.FromResult(response);
        }
    }
}


