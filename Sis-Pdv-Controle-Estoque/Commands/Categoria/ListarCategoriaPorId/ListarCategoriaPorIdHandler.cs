using MediatR;
using prmToolkit.NotificationPattern;

namespace Commands.Categoria.ListarCategoriaPorId
{
    public class ListarCategoriaPorIdHandler : Notifiable, IRequestHandler<ListarCategoriaPorIdRequest, ListarCategoriaPorIdResponse>
    {
        private readonly IRepositoryCategoria _repositoryCategoria;

        public ListarCategoriaPorIdHandler(IRepositoryCategoria repositoryCategoria)
        {
            _repositoryCategoria = repositoryCategoria;
        }

        public Task<ListarCategoriaPorIdResponse> Handle(ListarCategoriaPorIdRequest request, CancellationToken cancellationToken)
        {
            //Valida se o objeto request esta nulo
            if (request == null)
            {
                AddNotification("Request", "A requisição não pode ser nula.");
                return null;
            }

            Model.Categoria Collection = _repositoryCategoria.ObterPor(x => x.Id == request.Id);

            if (Collection == null)
            {
                AddNotification("Request", "A requisição não pode ser nula.");
                return null;
            }

            //Cria objeto de resposta
            var response = (ListarCategoriaPorIdResponse)Collection;

            ////Retorna o resultado
            return Task.FromResult(response);
        }
    }
}

