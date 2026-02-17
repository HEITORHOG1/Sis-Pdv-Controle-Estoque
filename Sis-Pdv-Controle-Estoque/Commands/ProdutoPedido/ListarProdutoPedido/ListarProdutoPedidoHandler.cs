using MediatR;
using prmToolkit.NotificationPattern;

namespace Commands.ProdutoPedido.ListarProdutoPedido
{
    public class ListarProdutoPedidoPorIdHandler : Notifiable, IRequestHandler<ListarProdutoPedidoRequest, Commands.Response>
    {
        private readonly IRepositoryProdutoPedido _repositoryProdutoPedido;

        public ListarProdutoPedidoPorIdHandler(IRepositoryProdutoPedido repositoryProdutoPedido)
        {
            _repositoryProdutoPedido = repositoryProdutoPedido;
        }

        public Task<Commands.Response> Handle(ListarProdutoPedidoRequest request, CancellationToken cancellationToken)
        {
            //Valida se o objeto request esta nulo
            if (request == null)
            {
                AddNotification("Request", "A requisição não pode ser nula.");
                return Task.FromResult(new Commands.Response(this));
            }

            var grupoCollection = _repositoryProdutoPedido.Listar().ToList();


            //Cria objeto de resposta
            var response = new Commands.Response(this, grupoCollection);

            ////Retorna o resultado
            return Task.FromResult(response);
        }
    }
}

