using Interfaces;
using MediatR;
using prmToolkit.NotificationPattern;

namespace Commands.Categoria.ListarCategoriaPorNomeCategoria
{
    public class ListarCategoriaPorNomeCategoriaHandler : Notifiable, IRequestHandler<ListarCategoriaPorNomeCategoriaRequest, ListarCategoriaPorNomeCategoriaResponse>
    {
        private readonly IRepositoryCategoria _repositoryCategoria;

        public ListarCategoriaPorNomeCategoriaHandler(IRepositoryCategoria repositoryCategoria)
        {
            _repositoryCategoria = repositoryCategoria;
        }

        public Task<ListarCategoriaPorNomeCategoriaResponse> Handle(ListarCategoriaPorNomeCategoriaRequest request, CancellationToken cancellationToken)
        {
            //Valida se o objeto request esta nulo
            if (request == null)
            {
                AddNotification("Erro", "Handle");
                return Task.FromResult(new ListarCategoriaPorNomeCategoriaResponse(this));
            }

            var Collection = _repositoryCategoria.Listar().Where(x => x.NomeCategoria == request.NomeCategoria);
            if (!Collection.Any())
            {
                AddNotification("ATENÇÃO", "CATEGORIA NÃO ENCONTRADA");
                return Task.FromResult(new ListarCategoriaPorNomeCategoriaResponse(this));
            }

            //Criar meu objeto de resposta
            var response = new ListarCategoriaPorNomeCategoriaResponse(this, Collection);
            //Cria objeto de resposta

            ////Retorna o resultado
            return Task.FromResult(response);
        }
    }
}


