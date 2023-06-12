using AdicionarProdutoPedido;
using Commands.ProdutoPedido.AdicionarProdutoPedido;
using MediatR;
using prmToolkit.NotificationPattern;
using Sis_Pdv_Controle_Estoque.Interfaces;

namespace Sis_Pdv_Controle_Estoque.Commands.AdicionarProdutoPedido
{
    public class AdicionarProdutoPedidoHandler : Notifiable, IRequestHandler<AdicionarProdutoPedidoRequest, Response>
    {
        private readonly IMediator _mediator;
        private readonly IRepositoryProdutoPedido _repositoryProdutoPedido;

        public AdicionarProdutoPedidoHandler(IMediator mediator, IRepositoryProdutoPedido repositoryProdutoPedido)
        {
            _mediator = mediator;
            _repositoryProdutoPedido = repositoryProdutoPedido;
        }

        public async Task<Response> Handle(AdicionarProdutoPedidoRequest request, CancellationToken cancellationToken)
        {
            // Instancia o validador
            var validator = new AdicionarProdutoPedidoRequestValidator();

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

            Model.ProdutoPedido ProdutoPedido = new(
                                                     request.PedidoId ,
                                                     request.ProdutoId,
                                                     request.codBarras,
                                                     request.quantidadeItemPedido,
                                                     request.totalProdutoPedido
                                                    );

            if (IsInvalid())
            {
                return new Response(this);
            }

            ProdutoPedido = _repositoryProdutoPedido.Adicionar(ProdutoPedido);

            //Criar meu objeto de resposta
            var response = new Response(this, ProdutoPedido);

            return await Task.FromResult(response);
        }
    }
}
