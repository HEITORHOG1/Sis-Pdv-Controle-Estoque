using MediatR;
using prmToolkit.NotificationPattern;

namespace Commands.Pedidos.RemoverPedido

{
    public class RemoverPedidoHandler : Notifiable, IRequestHandler<RemoverPedidoRequest, Commands.Response>
    {
        private readonly IRepositoryPedido _repositoryPedido;

        public RemoverPedidoHandler(IRepositoryPedido repositoryPedido)
        {
            _repositoryPedido = repositoryPedido;
        }

        public Task<Commands.Response> Handle(RemoverPedidoRequest request, CancellationToken cancellationToken)
        {
            //Valida se o objeto request esta nulo
            if (request == null)
            {
                AddNotification("Request", "A requisição não pode ser nula.");
                return Task.FromResult(new Commands.Response(this));
            }

            Model.Pedido Pedido = _repositoryPedido.ObterPorId(request.Id);

            if (Pedido == null)
            {
                AddNotification("Request", "A requisição não pode ser nula.");
                return Task.FromResult(new Commands.Response(this));
            }

            _repositoryPedido.Remover(Pedido);

            var result = new { Pedido.Id };

            //Cria objeto de resposta
            var response = new Commands.Response(this, result);

            ////Retorna o resultado
            return Task.FromResult(response);
        }
    }
}
