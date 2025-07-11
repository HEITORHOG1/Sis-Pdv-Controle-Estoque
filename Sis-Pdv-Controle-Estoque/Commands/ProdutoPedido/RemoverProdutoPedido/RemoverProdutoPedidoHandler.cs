﻿using MediatR;
using prmToolkit.NotificationPattern;

namespace Commands.ProdutoPedido.RemoverProdutoPedido

{
    public class RemoverProdutoPedidoHandler : Notifiable, IRequestHandler<RemoverProdutoPedidoResquest, Commands.Response>
    {
        private readonly IMediator _mediator;
        private readonly IRepositoryProdutoPedido _repositoryProdutoPedido;

        public RemoverProdutoPedidoHandler(IMediator mediator, IRepositoryProdutoPedido repositoryProdutoPedido)
        {
            _mediator = mediator;
            _repositoryProdutoPedido = repositoryProdutoPedido;
        }

        public async Task<Commands.Response> Handle(RemoverProdutoPedidoResquest request, CancellationToken cancellationToken)
        {
            //Valida se o objeto request esta nulo
            if (request == null)
            {
                AddNotification("Request", "");
                return new Commands.Response(this);
            }

            Model.ProdutoPedido ProdutoPedido = _repositoryProdutoPedido.ObterPorId(request.Id);

            if (ProdutoPedido == null)
            {
                AddNotification("Request", "");
                return new Commands.Response(this);
            }

            _repositoryProdutoPedido.Remover(ProdutoPedido);

            var result = new { Id = ProdutoPedido.Id };

            //Cria objeto de resposta
            var response = new Commands.Response(this, result);

            ////Retorna o resultado
            return await Task.FromResult(response);
        }
    }
}
