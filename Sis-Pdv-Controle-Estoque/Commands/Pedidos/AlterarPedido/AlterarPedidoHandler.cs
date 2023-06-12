using AlterarPedido;
using MediatR;
using prmToolkit.NotificationPattern;
using Sis_Pdv_Controle_Estoque.Interfaces;
using Response = Sis_Pdv_Controle_Estoque.Commands.Response;

namespace Commands.Pedido.AlterarPedido
{
    public class AlterarPedidoHandler : Notifiable, IRequestHandler<AlterarPedidoRequest, Response>
    {
        private readonly IMediator _mediator;
        private readonly IRepositoryPedido _repositoryPedido;

        public AlterarPedidoHandler(IMediator mediator, IRepositoryPedido repositoryPedido)
        {
            _mediator = mediator;
            _repositoryPedido = repositoryPedido;
        }

        public async Task<Response> Handle(AlterarPedidoRequest request, CancellationToken cancellationToken)
        {
            // Instancia o validador
            var validator = new AlterarPedidoRequestValidator();

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

            Sis_Pdv_Controle_Estoque.Model.Pedido Pedido = new Sis_Pdv_Controle_Estoque.Model.Pedido();

            Pedido.AlterarPedido(       request.ColaboradorId,
                                        request.ClienteId,
                                        request.Status,
                                        request.dataDoPedido,
                                        request.formaPagamento,
                                        request.totalPedido     );

            var retornoExist = _repositoryPedido.Listar().Where(x => x.Id == request.Id);
            if (!retornoExist.Any())
            {
                AddNotification("Pedido", "");
                return new Response(this);
            }

            Pedido = _repositoryPedido.Editar(Pedido);

            //Criar meu objeto de resposta
            var response = new Response(this, Pedido);

            return await Task.FromResult(response);
        }
    }
}
