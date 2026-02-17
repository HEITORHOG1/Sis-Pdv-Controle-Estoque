using Interfaces;
using MediatR;
using prmToolkit.NotificationPattern;

namespace Commands.Colaborador.ListarColaboradorPorNomeColaborador
{
    public class ListarColaboradorPorNomeColaboradorHandler : Notifiable, IRequestHandler<ListarColaboradorPorNomeColaboradorRequest, ListarColaboradorPorNomeColaboradorResponse>
    {
        private readonly IRepositoryColaborador _repositoryColaborador;

        public ListarColaboradorPorNomeColaboradorHandler(IRepositoryColaborador repositoryColaborador)
        {
            _repositoryColaborador = repositoryColaborador;
        }

        public Task<ListarColaboradorPorNomeColaboradorResponse> Handle(ListarColaboradorPorNomeColaboradorRequest request, CancellationToken cancellationToken)
        {
            //Valida se o objeto request esta nulo
            if (request == null)
            {
                AddNotification("Erro", "Handle");
                return Task.FromResult(new ListarColaboradorPorNomeColaboradorResponse(this));
            }

            var Collection = _repositoryColaborador.Listar().Where(x => x.NomeColaborador == request.NomeColaborador);
            if (!Collection.Any())
            {
                AddNotification("ATENÇÃO", "Colaborador NÃO ENCONTRADA");
                return Task.FromResult(new ListarColaboradorPorNomeColaboradorResponse(this));
            }

            //Criar meu objeto de resposta
            var response = new ListarColaboradorPorNomeColaboradorResponse(this, Collection);
            //Cria objeto de resposta

            ////Retorna o resultado
            return Task.FromResult(response);
        }
    }
}


