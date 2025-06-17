using MediatR;
using prmToolkit.NotificationPattern;

namespace Commands.Produto.ListarProdutoPorNomeProduto
{
    public class ListarProdutoPorCodBarrasHandler : Notifiable, IRequestHandler<ListarProdutoPorCodBarrasRequest, Commands.Response>
    {
        private readonly IMediator _mediator;
        private readonly IRepositoryProduto _repositoryProduto;

        public ListarProdutoPorCodBarrasHandler(IMediator mediator, IRepositoryProduto repositoryProduto)
        {
            _mediator = mediator;
            _repositoryProduto = repositoryProduto;
        }

        public async Task<Commands.Response> Handle(ListarProdutoPorCodBarrasRequest request, CancellationToken cancellationToken)
        {
            //Valida se o objeto request esta nulo
            if (request == null)
            {
                AddNotification("Erro", "Handle");
                return new Commands.Response(this);
            }

            var Collection = _repositoryProduto.Listar().Where(x => x.codBarras == request.codBarras);
            if (!Collection.Any())
            {
                AddNotification("ATENÇÃO", "PRODUTO NÃO ENCONTRADA");
                return new Commands.Response(this);
            }

            //Criar meu objeto de resposta
            var response = new Commands.Response(this, Collection);
            //Cria objeto de resposta

            ////Retorna o resultado
            return await Task.FromResult(response);
        }
    }
}


