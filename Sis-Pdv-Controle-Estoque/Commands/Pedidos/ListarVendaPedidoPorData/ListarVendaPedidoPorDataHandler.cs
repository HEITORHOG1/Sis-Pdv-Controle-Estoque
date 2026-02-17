using MediatR;
using prmToolkit.NotificationPattern;

namespace Commands.Pedidos.ListarVendaPedidoPorData
{
    public class ListarVendaPedidoPorDataHandler : Notifiable, IRequestHandler<ListarVendaPedidoPorDataRequest, Commands.Response>
    {
        private readonly IRepositoryPedido _repositoryPedido;

        public ListarVendaPedidoPorDataHandler(IRepositoryPedido repositoryPedido)
        {
            _repositoryPedido = repositoryPedido;
        }

        public Task<Commands.Response> Handle(ListarVendaPedidoPorDataRequest request, CancellationToken cancellationToken)
        {
            //Valida se o objeto request esta nulo
            if (request == null)
            {
                AddNotification("Erro", "Handle");
                return Task.FromResult(new Commands.Response(this));
            }

            var Collection = _repositoryPedido.ListarVendaPedidoPorData(request.DataInicio, request.DataFim);

            if (!Collection.Result.Any())
            {
                AddNotification("ATENÇÃO", "Pedido NÃO ENCONTRADA");
                return Task.FromResult(new Commands.Response(this));
            }

            //Criar meu objeto de resposta
            var response = new Commands.Response(this, Collection.Result);
            //Cria objeto de resposta

            ////Retorna o resultado
            return Task.FromResult(response);
        }
    }
}


