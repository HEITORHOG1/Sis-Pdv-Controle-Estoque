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

            if (IsInvalid())
            {
                return new Response(this);
            }

            Model.Categoria categoria = new(request.NomeCategoria);
            categoria = await _repositoryCategoria.AdicionarAsync(categoria, cancellationToken);

            //Criar meu objeto de resposta
            var result = new { categoria.Id, categoria.NomeCategoria };

            //Criar meu objeto de resposta
            var response = new Response(this, result);

            return response;
        }
    }
}

