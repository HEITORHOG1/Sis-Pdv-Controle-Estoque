using MediatR;
using prmToolkit.NotificationPattern;

namespace Commands.Pedidos.AlterarPedido
{
    public class AlterarPedidoHandler : Notifiable, IRequestHandler<AlterarPedidoRequest, Response>
    {
        private readonly IRepositoryPedido _repositoryPedido;

        public AlterarPedidoHandler(IRepositoryPedido repositoryPedido)
        {
            _repositoryPedido = repositoryPedido;
        }

        public async Task<Response> Handle(AlterarPedidoRequest request, CancellationToken cancellationToken)
        {

            Model.Pedido Pedido = new Model.Pedido();

            Pedido.AlterarPedido(request.ColaboradorId,
                                        request.ClienteId,
                                        request.Status,
                                        request.DataDoPedido,
                                        request.FormaPagamento,
                                        request.TotalPedido);

            var retornoExist = _repositoryPedido.Listar().Where(x => x.Id == request.Id);
            if (!retornoExist.Any())
            {
                AddNotification("Pedido", "Pedido nï¿½o encontrado com o ID informado.");
                return new Response(this);
            }

            Pedido = await _repositoryPedido.EditarAsync(Pedido, cancellationToken);

            //Criar meu objeto de resposta
            var response = new Response(this, Pedido);

            return response;
        }
    }
}

