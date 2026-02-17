using MediatR;
using prmToolkit.NotificationPattern;

namespace Commands.Pedidos.AdicionarPedido
{
    public class AdicionarPedidoHandler : Notifiable, IRequestHandler<AdicionarPedidoRequest, Response>
    {
        private readonly IRepositoryPedido _repositoryPedido;

        public AdicionarPedidoHandler(IRepositoryPedido repositoryPedido)
        {
            _repositoryPedido = repositoryPedido;
        }

        public Task<Response> Handle(AdicionarPedidoRequest request, CancellationToken cancellationToken)
        {
            // Validation is now handled by the ValidationBehavior pipeline

            Model.Pedido Pedido = new(
                                        request.ColaboradorId,
                                        request.ClienteId,
                                        request.Status,
                                        request.DataDoPedido,
                                        request.FormaPagamento,
                                        request.TotalPedido);

            if (IsInvalid())
            {
                return Task.FromResult(new Response(this));
            }

            Pedido = _repositoryPedido.Adicionar(Pedido);

            //Criar meu objeto de resposta
            var response = new Response(this, Pedido);

            return Task.FromResult(response);
        }
    }
}
