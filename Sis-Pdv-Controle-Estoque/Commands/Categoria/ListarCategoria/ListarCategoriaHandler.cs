using Interfaces;
using MediatR;
using prmToolkit.NotificationPattern;

namespace Commands.Categoria.ListarCategoria
{
    public class ListarCategoriaHandler : Notifiable, IRequestHandler<ListarCategoriaRequest, Response>
    {
        private readonly IMediator _mediator;
        private readonly IRepositoryCategoria _repositoryCategoria;

        public ListarCategoriaHandler(IMediator mediator, IRepositoryCategoria repositoryCategoria)
        {
            _mediator = mediator;
            _repositoryCategoria = repositoryCategoria;
        }

        public async Task<Response> Handle(ListarCategoriaRequest request, CancellationToken cancellationToken)
        {
            //Valida se o objeto request esta nulo
            if (request == null)
            {
                AddNotification("Request", "");
                return new Response(this);
            }

            var grupoCollection = _repositoryCategoria.Listar().ToList();


            //Cria objeto de resposta
            var response = new Response(this, grupoCollection);

            ////Retorna o resultado
            return await Task.FromResult(response);
        }
    }
}

