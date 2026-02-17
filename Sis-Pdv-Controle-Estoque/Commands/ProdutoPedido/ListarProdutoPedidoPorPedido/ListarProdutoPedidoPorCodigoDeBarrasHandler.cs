using MediatR;
using prmToolkit.NotificationPattern;

namespace Commands.ProdutoPedido.ListarProdutoPedidoPorPedido
{
    public class ListarProdutoPedidoPorCodigoDeBarrasHandler : Notifiable, IRequestHandler<ListarProdutoPedidoPorCodigoDeBarrasRequest, Commands.Response>
    {
        private readonly IRepositoryProdutoPedido _repositoryProdutoPedido;

        public ListarProdutoPedidoPorCodigoDeBarrasHandler(IRepositoryProdutoPedido repositoryProdutoPedido)
        {
            _repositoryProdutoPedido = repositoryProdutoPedido;
        }

        public Task<Commands.Response> Handle(ListarProdutoPedidoPorCodigoDeBarrasRequest request, CancellationToken cancellationToken)
        {
            //Valida se o objeto request esta nulo
            if (request == null)
            {
                AddNotification("Erro", "Handle");
                return Task.FromResult(new Commands.Response(this));
            }

            var Collection = _repositoryProdutoPedido.Listar()
                                .Where(x => x.CodBarras == request.CodBarras);
            if (!Collection.Any())
            {
                AddNotification("ATENÇÃO", "ProdutoPedido NÃO ENCONTRADA");
                return Task.FromResult(new Commands.Response(this));
            }

            //Criar meu objeto de resposta
            var response = new Commands.Response(this, Collection);
            //Cria objeto de resposta

            ////Retorna o resultado
            return Task.FromResult(response);
        }
    }
}


