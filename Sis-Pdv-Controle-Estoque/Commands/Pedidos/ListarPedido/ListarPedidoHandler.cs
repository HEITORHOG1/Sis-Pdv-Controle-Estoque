using MediatR;
using prmToolkit.NotificationPattern;

namespace Commands.Pedidos.ListarPedido
{
    public class ListarPedidoPorIdHandler : Notifiable, IRequestHandler<ListarPedidoRequest, Commands.Response>
    {
        private readonly IRepositoryPedido _repositoryPedido;

        public ListarPedidoPorIdHandler(IRepositoryPedido repositoryPedido)
        {
            _repositoryPedido = repositoryPedido;
        }

        public async Task<Commands.Response> Handle(ListarPedidoRequest request, CancellationToken cancellationToken)
        {
            // Valida se o objeto request esta nulo
            if (request == null)
            {
                AddNotification("Request", "A requisição não pode ser nula.");
                return new Commands.Response(this);
            }

            // Lista Pedidos incluindo o Colaborador
            var grupoCollection = await _repositoryPedido.ListarAsync(cancellationToken, x => x.Colaborador);

            // Cria objeto de resposta
            var response = new Commands.Response(this, grupoCollection);

            // Retorna o resultado
            return response;
        }
    }
}
