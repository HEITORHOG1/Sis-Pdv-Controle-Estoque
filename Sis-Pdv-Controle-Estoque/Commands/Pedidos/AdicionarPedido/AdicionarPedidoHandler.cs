using MediatR;
using prmToolkit.NotificationPattern;

namespace Commands.Pedidos.AdicionarPedido
{
    public class AdicionarPedidoHandler : Notifiable, IRequestHandler<AdicionarPedidoRequest, Response>
    {
        private readonly IMediator _mediator;
        private readonly IRepositoryPedido _repositoryPedido;

        public AdicionarPedidoHandler(IMediator mediator, IRepositoryPedido repositoryPedido)
        {
            _mediator = mediator;
            _repositoryPedido = repositoryPedido;
        }

        public async Task<Response> Handle(AdicionarPedidoRequest request, CancellationToken cancellationToken)
        {
            // Validation is now handled by the ValidationBehavior pipeline

            Model.Pedido Pedido = new(
                                        request.ColaboradorId,
                                        request.ClienteId,
                                        request.Status,
                                        request.dataDoPedido,
                                        request.formaPagamento,
                                        request.totalPedido);

            if (IsInvalid())
            {
                return new Response(this);
            }

            Pedido = _repositoryPedido.Adicionar(Pedido);

            //Criar meu objeto de resposta
            var response = new Response(this, Pedido);

            return await Task.FromResult(response);
        }
    }
}
