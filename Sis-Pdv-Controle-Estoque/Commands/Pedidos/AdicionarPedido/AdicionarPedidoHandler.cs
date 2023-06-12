using AdicionarPedido;
using Commands.Pedido.AdicionarPedido;
using MediatR;
using prmToolkit.NotificationPattern;
using Sis_Pdv_Controle_Estoque.Interfaces;

namespace Sis_Pdv_Controle_Estoque.Commands.AdicionarPedido
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
            // Instancia o validador
            var validator = new AdicionarPedidoRequestValidator();

            // Valida a requisição
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            // Se não passou na validação, adiciona as falhas como notificações
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    AddNotification(error.PropertyName, error.ErrorMessage);
                }

                return new Response(this);
            }

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
