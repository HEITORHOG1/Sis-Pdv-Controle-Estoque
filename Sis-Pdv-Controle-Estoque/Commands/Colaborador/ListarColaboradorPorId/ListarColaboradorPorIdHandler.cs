using Interfaces;
using MediatR;
using prmToolkit.NotificationPattern;

namespace Commands.Colaborador.ListarColaboradorPorId
{
    public class ListarColaboradorPorIdHandler : Notifiable, IRequestHandler<ListarColaboradorPorIdRequest, ListarColaboradorPorIdResponse>
    {
        private readonly IRepositoryColaborador _repositoryColaborador;

        public ListarColaboradorPorIdHandler(IRepositoryColaborador repositoryColaborador)
        {
            _repositoryColaborador = repositoryColaborador;
        }

        public Task<ListarColaboradorPorIdResponse> Handle(ListarColaboradorPorIdRequest request, CancellationToken cancellationToken)
        {
            //Valida se o objeto request esta nulo
            if (request == null)
            {
                AddNotification("Request", "A requisição não pode ser nula.");
                return null;
            }

            Model.Colaborador Collection = _repositoryColaborador.ObterPor(x => x.Id == request.Id);

            if (Collection == null)
            {
                AddNotification("Request", "A requisição não pode ser nula.");
                return null;
            }

            //Cria objeto de resposta
            var response = (ListarColaboradorPorIdResponse)Collection;

            ////Retorna o resultado
            return Task.FromResult(response);
        }
    }
}

