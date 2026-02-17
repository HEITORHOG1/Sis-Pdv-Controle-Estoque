using Interfaces;
using MediatR;
using prmToolkit.NotificationPattern;

namespace Commands.Categoria.ListarCategoria
{
    public class ListarCategoriaHandler : Notifiable, IRequestHandler<ListarCategoriaRequest, Response>
    {
        private readonly IRepositoryCategoria _repositoryCategoria;

        public ListarCategoriaHandler(IRepositoryCategoria repositoryCategoria)
        {
            _repositoryCategoria = repositoryCategoria;
        }

        public Task<Response> Handle(ListarCategoriaRequest request, CancellationToken cancellationToken)
        {
            //Valida se o objeto request esta nulo
            if (request == null)
            {
                AddNotification("Request", "A requisição não pode ser nula.");
                return Task.FromResult(new Response(this));
            }

            var grupoCollection = _repositoryCategoria.Listar().ToList();


            //Cria objeto de resposta
            var response = new Response(this, grupoCollection);

            ////Retorna o resultado
            return Task.FromResult(response);
        }
    }
}

