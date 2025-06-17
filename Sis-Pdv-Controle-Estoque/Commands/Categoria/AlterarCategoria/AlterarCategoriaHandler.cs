using Interfaces;
using MediatR;
using prmToolkit.NotificationPattern;

namespace Commands.Categoria.AlterarCategoria
{
    public class AlterarCategoriaHandler : Notifiable, IRequestHandler<AlterarCategoriaRequest, Response>
    {
        private readonly IMediator _mediator;
        private readonly IRepositoryCategoria _repositoryCategoria;

        public AlterarCategoriaHandler(IMediator mediator, IRepositoryCategoria repositoryCategoria)
        {
            _mediator = mediator;
            _repositoryCategoria = repositoryCategoria;
        }

        public async Task<Response> Handle(AlterarCategoriaRequest request, CancellationToken cancellationToken)
        {
            // Instancia o validador
            var validator = new AlterarCategoriaRequestValidator();

            // Valida a requisição
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            // Se não passou na validação, adiciona as falhas como notificações
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    AddNotification(error.PropertyName, error.ErrorMessage);
                }

                return new Response(this);
            }

            Model.Categoria categoria = new Model.Categoria();

            categoria.AlterarCategoria(request.Id, request.NomeCategoria);
            var retornoExist = _repositoryCategoria.Listar().Where(x => x.Id == request.Id);
            if (!retornoExist.Any())
            {
                AddNotification("Atenção", "Categoria não encontrada");
                return new Response(this);
            }

            if (categoria == null)
            {
                AddNotification("Categoria", "categoria == null");
                return new Response(this);
            }

            categoria = _repositoryCategoria.Editar(categoria);

            var result = new { Id = categoria.Id, NomeCategoria = categoria.NomeCategoria };

            //Criar meu objeto de resposta
            var response = new Response(this, result);

            return await Task.FromResult(response);
        }
    }
}
