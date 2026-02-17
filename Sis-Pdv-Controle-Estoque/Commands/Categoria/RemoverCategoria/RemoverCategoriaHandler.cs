using MediatR;
using prmToolkit.NotificationPattern;

namespace Commands.Categoria.RemoverCategoria
{
    public class RemoverCategoriaHandler : Notifiable, IRequestHandler<RemoverCategoriaRequest, Response>
    {
        private readonly IRepositoryCategoria _repositoryCategoria;

        public RemoverCategoriaHandler(IRepositoryCategoria repositoryCategoria)
        {
            _repositoryCategoria = repositoryCategoria;
        }

        public Task<Response> Handle(RemoverCategoriaRequest request, CancellationToken cancellationToken)
        {
            //Valida se o objeto request esta nulo
            if (request == null)
            {
                AddNotification("Request", "Request não pode ser nulo");
                return Task.FromResult(new Response(this));
            }

            var categoria = _repositoryCategoria.ObterPorId(request.Id);

            if (categoria == null)
            {
                AddNotification("Categoria", "CATEGORIA NÃO ENCONTRADA");
                return Task.FromResult(new Response(this));
            }

            // Verifica se a categoria já foi deletada
            if (categoria.IsDeleted)
            {
                AddNotification("Categoria", "CATEGORIA JÁ FOI REMOVIDA");
                return Task.FromResult(new Response(this));
            }

            // Realiza o soft delete
            _repositoryCategoria.Remover(categoria);

            //Criar meu objeto de resposta
            var response = new Response(this, categoria);

            ////Retorna o resultado
            return Task.FromResult(response);
        }
    }
}
