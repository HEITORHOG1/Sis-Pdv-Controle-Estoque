using MediatR;
using prmToolkit.NotificationPattern;

namespace Commands.Categoria.AdicionarCategoria
{
    public class AdicionarCategoriaHandler : Notifiable, IRequestHandler<AdicionarCategoriaRequest, Response>
    {
        private readonly IRepositoryCategoria _repositoryCategoria;

        public AdicionarCategoriaHandler(IRepositoryCategoria repositoryCategoria)
        {
            _repositoryCategoria = repositoryCategoria;
        }

        public async Task<Response> Handle(AdicionarCategoriaRequest request, CancellationToken cancellationToken)
        {
            // Instancia o validador
            var validator = new AdicionarCategoriaValidator();

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

            if (IsInvalid())
            {
                return new Response(this);
            }

            Model.Categoria categoria = new(request.NomeCategoria);
            categoria = _repositoryCategoria.Adicionar(categoria);

            //Criar meu objeto de resposta
            var result = new { categoria.Id, categoria.NomeCategoria };

            //Criar meu objeto de resposta
            var response = new Response(this, result);

            return await Task.FromResult(response);
        }
    }
}
