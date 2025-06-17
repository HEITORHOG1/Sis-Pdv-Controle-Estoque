using Interfaces;
using MediatR;
using prmToolkit.NotificationPattern;

namespace Commands.Colaborador.ListarColaboradorPorNomeColaborador
{
    public class ListarColaboradorPorNomeColaboradorHandler : Notifiable, IRequestHandler<ListarColaboradorPorNomeColaboradorRequest, ListarColaboradorPorNomeColaboradorResponse>
    {
        private readonly IMediator _mediator;
        private readonly IRepositoryColaborador _repositoryColaborador;

        public ListarColaboradorPorNomeColaboradorHandler(IMediator mediator, IRepositoryColaborador repositoryColaborador)
        {
            _mediator = mediator;
            _repositoryColaborador = repositoryColaborador;
        }

        public async Task<ListarColaboradorPorNomeColaboradorResponse> Handle(ListarColaboradorPorNomeColaboradorRequest request, CancellationToken cancellationToken)
        {
            //Valida se o objeto request esta nulo
            if (request == null)
            {
                AddNotification("Erro", "Handle");
                return new ListarColaboradorPorNomeColaboradorResponse(this);
            }

            var Collection = _repositoryColaborador.Listar().Where(x => x.nomeColaborador == request.NomeColaborador);
            if (!Collection.Any())
            {
                AddNotification("ATENÇÃO", "Colaborador NÃO ENCONTRADA");
                return new ListarColaboradorPorNomeColaboradorResponse(this);
            }

            //Criar meu objeto de resposta
            var response = new ListarColaboradorPorNomeColaboradorResponse(this, Collection);
            //Cria objeto de resposta

            ////Retorna o resultado
            return await Task.FromResult(response);
        }
    }
}


