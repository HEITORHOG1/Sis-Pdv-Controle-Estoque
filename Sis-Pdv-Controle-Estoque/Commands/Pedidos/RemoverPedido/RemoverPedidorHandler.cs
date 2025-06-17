using MediatR;
using prmToolkit.NotificationPattern;

namespace Commands.Pedidos.RemoverPedido

{
    public class RemoverPedidoHandler : Notifiable, IRequestHandler<RemoverPedidoResquest, Sis_Pdv_Controle_Estoque.Commands.Response>
    {
        private readonly IMediator _mediator;
        private readonly IRepositoryPedido _repositoryPedido;

        public RemoverPedidoHandler(IMediator mediator, IRepositoryPedido repositoryPedido)
        {
            _mediator = mediator;
            _repositoryPedido = repositoryPedido;
        }

        public async Task<Sis_Pdv_Controle_Estoque.Commands.Response> Handle(RemoverPedidoResquest request, CancellationToken cancellationToken)
        {
            //Valida se o objeto request esta nulo
            if (request == null)
            {
                AddNotification("Request", "");
                return new Sis_Pdv_Controle_Estoque.Commands.Response(this);
            }

            Sis_Pdv_Controle_Estoque.Model.Pedido Pedido = _repositoryPedido.ObterPorId(request.Id);

            if (Pedido == null)
            {
                AddNotification("Request", "");
                return new Sis_Pdv_Controle_Estoque.Commands.Response(this);
            }

            _repositoryPedido.Remover(Pedido);

            var result = new { Pedido.Id };

            //Cria objeto de resposta
            var response = new Sis_Pdv_Controle_Estoque.Commands.Response(this, result);

            ////Retorna o resultado
            return await Task.FromResult(response);
        }
    }
}
