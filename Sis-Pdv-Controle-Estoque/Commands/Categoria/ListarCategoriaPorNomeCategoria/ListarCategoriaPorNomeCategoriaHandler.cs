using Interfaces;
using MediatR;
using prmToolkit.NotificationPattern;

namespace Commands.Categoria.ListarCategoriaPorNomeCategoria
{
    public class ListarCategoriaPorNomeCategoriaHandler : Notifiable, IRequestHandler<ListarCategoriaPorNomeCategoriaRequest, ListarCategoriaPorNomeCategoriaResponse>
    {
        private readonly IMediator _mediator;
        private readonly IRepositoryCategoria _repositoryCategoria;

        public ListarCategoriaPorNomeCategoriaHandler(IMediator mediator, IRepositoryCategoria repositoryCategoria)
        {
            _mediator = mediator;
            _repositoryCategoria = repositoryCategoria;
        }

        public async Task<ListarCategoriaPorNomeCategoriaResponse> Handle(ListarCategoriaPorNomeCategoriaRequest request, CancellationToken cancellationToken)
        {
            //Valida se o objeto request esta nulo
            if (request == null)
            {
                AddNotification("Erro", "Handle");
                return new ListarCategoriaPorNomeCategoriaResponse(this);
            }

            var Collection = _repositoryCategoria.Listar().Where(x => x.NomeCategoria == request.NomeCategoria);
            if (!Collection.Any())
            {
                AddNotification("ATENÇÃO", "CATEGORIA NÃO ENCONTRADA");
                return new ListarCategoriaPorNomeCategoriaResponse(this);
            }

            //Criar meu objeto de resposta
            var response = new ListarCategoriaPorNomeCategoriaResponse(this, Collection);
            //Cria objeto de resposta

            ////Retorna o resultado
            return await Task.FromResult(response);
        }
    }
}


