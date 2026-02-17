using MediatR;
using prmToolkit.NotificationPattern;

namespace Commands.ProdutoPedido.RemoverProdutoPedido

{
    public class RemoverProdutoPedidoHandler : Notifiable, IRequestHandler<RemoverProdutoPedidoRequest, Commands.Response>
    {
        private readonly IRepositoryProdutoPedido _repositoryProdutoPedido;

        public RemoverProdutoPedidoHandler(IRepositoryProdutoPedido repositoryProdutoPedido)
        {
            _repositoryProdutoPedido = repositoryProdutoPedido;
        }

        public Task<Commands.Response> Handle(RemoverProdutoPedidoRequest request, CancellationToken cancellationToken)
        {
            //Valida se o objeto request esta nulo
            if (request == null)
            {
                AddNotification("Request", "A requisição não pode ser nula.");
                return Task.FromResult(new Commands.Response(this));
            }

            Model.ProdutoPedido ProdutoPedido = _repositoryProdutoPedido.ObterPorId(request.Id);

            if (ProdutoPedido == null)
            {
                AddNotification("Request", "A requisição não pode ser nula.");
                return Task.FromResult(new Commands.Response(this));
            }

            _repositoryProdutoPedido.Remover(ProdutoPedido);

            var result = new { Id = ProdutoPedido.Id };

            //Cria objeto de resposta
            var response = new Commands.Response(this, result);

            ////Retorna o resultado
            return Task.FromResult(response);
        }
    }
}
