using Interfaces;
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
                AddNotification("Request", "");
                return new Commands.Response(this);
            }

            Model.Categoria categoria = _repositoryCategoria.ObterPorId(request.Id);

            if (categoria == null)
            {
                AddNotification("Request", "");
                return new Commands.Response(this);
            }

            _repositoryCategoria.Remover(categoria);

            var result = new { Id = categoria.Id };

            //Cria objeto de resposta
            var response = new Commands.Response(this, result);


            Console.WriteLine($"Response created with Id: {categoria.Id}");

            ////Retorna o resultado
            return await Task.FromResult(response);
        }
    }
}
