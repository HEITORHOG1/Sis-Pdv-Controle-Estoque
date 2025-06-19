using MediatR;
using prmToolkit.NotificationPattern;

namespace Commands.Categoria.ListarCategoriaPorId
{
    public class ListarCategoriaPorIdHandler : Notifiable, IRequestHandler<ListarCategoriaPorIdRequest, ListarCategoriaPorIdResponse>
    {
        private readonly IMediator _mediator;
        private readonly IRepositoryCategoria _repositoryCategoria;

        public ListarCategoriaPorIdHandler(IMediator mediator, IRepositoryCategoria repositoryCategoria)
        {
            _mediator = mediator;
            _repositoryCategoria = repositoryCategoria;
        }

        public async Task<ListarCategoriaPorIdResponse> Handle(ListarCategoriaPorIdRequest request, CancellationToken cancellationToken)
        {
            //Valida se o objeto request esta nulo
            if (request == null)
            {
                AddNotification("Request", "");
                return null;
            }

            Model.Categoria Collection = _repositoryCategoria.ObterPor(x => x.Id == request.Id);

            if (Collection == null)
            {
                AddNotification("Request", "");
                return null;
            }

            //Cria objeto de resposta
            var response = (ListarCategoriaPorIdResponse)Collection;

            ////Retorna o resultado
            return await Task.FromResult(response);
        }
    }
}

