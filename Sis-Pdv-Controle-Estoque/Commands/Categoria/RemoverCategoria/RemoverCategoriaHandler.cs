using MediatR;
using prmToolkit.NotificationPattern;

namespace Commands.Categoria.RemoverCategoria
{
    public class RemoverCategoriaHandler : Notifiable, IRequestHandler<RemoverCategoriaResquest, Response>
    {
        private readonly IMediator _mediator;
        private readonly IRepositoryCategoria _repositoryCategoria;

        public RemoverCategoriaHandler(IMediator mediator, IRepositoryCategoria repositoryCategoria)
        {
            _mediator = mediator;
            _repositoryCategoria = repositoryCategoria;
        }

        public async Task<Response> Handle(RemoverCategoriaResquest request, CancellationToken cancellationToken)
        {
            //Valida se o objeto request esta nulo
            if (request == null)
            {
                AddNotification("Request", "Request não pode ser nulo");
                return new Response(this);
            }

            var categoria = _repositoryCategoria.ObterPorId(request.Id);

            if (categoria == null)
            {
                AddNotification("Categoria", "CATEGORIA NÃO ENCONTRADA");
                return new Response(this);
            }

            // Verifica se a categoria já foi deletada
            if (categoria.IsDeleted)
            {
                AddNotification("Categoria", "CATEGORIA JÁ FOI REMOVIDA");
                return new Response(this);
            }

            // Realiza o soft delete
            _repositoryCategoria.Remover(categoria);

            //Criar meu objeto de resposta
            var response = new Response(this, categoria);

            ////Retorna o resultado
            return await Task.FromResult(response);
        }
    }
}
