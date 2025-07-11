﻿using MediatR;
using prmToolkit.NotificationPattern;

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

            Model.ProdutoPedido ProdutoPedido = new Model.ProdutoPedido();

            ProdutoPedido.AlterarProdutoPedido(
                                                request.PedidoId,
                                                request.ProdutoId,
                                                request.codBarras,
                                                request.quantidadeItemPedido,
                                                request.totalProdutoPedido
                );

            var retornoExist = _repositoryProdutoPedido.Listar().Where(x => x.Id == request.Id);
            if (!retornoExist.Any())
            {
                AddNotification("ProdutoPedido", "");
                return new Response(this);
            }

            ProdutoPedido = _repositoryProdutoPedido.Editar(ProdutoPedido);

            //Criar meu objeto de resposta
            var response = new Commands.Response(this, ProdutoPedido);

            return await Task.FromResult(response);
        }
    }
}
