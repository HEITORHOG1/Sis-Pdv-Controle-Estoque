using MediatR;
using prmToolkit.NotificationPattern;

namespace Commands.Pedidos.ListarPedidoPorId
{
    public class ListarPedidoPorIdHandler : Notifiable, IRequestHandler<ListarPedidoPorIdRequest, Commands.Response>
    {
        private readonly IRepositoryPedido _repositoryPedido;

        public ListarPedidoPorIdHandler(IRepositoryPedido repositoryPedido)
        {
            _repositoryPedido = repositoryPedido;
        }

        public Task<Commands.Response> Handle(ListarPedidoPorIdRequest request, CancellationToken cancellationToken)
        {
            //Valida se o objeto request esta nulo
            if (request == null)
            {
                AddNotification("Request", "A requisi��o n�o pode ser nula.");
                return Task.FromResult(new Commands.Response(this));
            }

            Model.Pedido Collection = _repositoryPedido.ListarPor(x => x.Id == request.Id).FirstOrDefault();

            if (Collection == null)
            {
                AddNotification("Request", "A requisi��o n�o pode ser nula.");
                return Task.FromResult(new Commands.Response(this));
            }

            //Cria objeto de resposta
            var response = new Commands.Response(this, Collection);

            ////Retorna o resultado
            return Task.FromResult(response);
        }
    }
}

