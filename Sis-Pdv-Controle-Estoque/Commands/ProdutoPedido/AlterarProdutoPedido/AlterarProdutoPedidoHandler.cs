using AlterarProdutoPedido;
using MediatR;
using prmToolkit.NotificationPattern;
using Sis_Pdv_Controle_Estoque.Interfaces;
using Response = Sis_Pdv_Controle_Estoque.Commands.Response;

namespace Commands.ProdutoPedido.AlterarProdutoPedido
{
    public class AlterarProdutoPedidoHandler : Notifiable, IRequestHandler<AlterarProdutoPedidoRequest, Response>
    {
        private readonly IMediator _mediator;
        private readonly IRepositoryProdutoPedido _repositoryProdutoPedido;

        public AlterarProdutoPedidoHandler(IMediator mediator, IRepositoryProdutoPedido repositoryProdutoPedido)
        {
            _mediator = mediator;
            _repositoryProdutoPedido = repositoryProdutoPedido;
        }

        public async Task<Response> Handle(AlterarProdutoPedidoRequest request, CancellationToken cancellationToken)
        {
            // Instancia o validador
            var validator = new AlterarProdutoPedidoRequestValidator();

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

            var produtoPedido = _repositoryProdutoPedido.ObterPorId(request.Id);
            if (produtoPedido == null)
            {
                AddNotification("ProdutoPedido", "Registro não encontrado");
                return new Response(this);
            }

            produtoPedido.AlterarProdutoPedido(
                                                request.PedidoId,
                                                request.ProdutoId,
                                                request.codBarras,
                                                request.quantidadeItemPedido,
                                                request.totalProdutoPedido
                );

            produtoPedido = _repositoryProdutoPedido.Editar(produtoPedido);

            //Criar meu objeto de resposta
            var response = new Sis_Pdv_Controle_Estoque.Commands.Response(this, produtoPedido);

            return await Task.FromResult(response);
        }
    }
}
