using MediatR;
using prmToolkit.NotificationPattern;

namespace Commands.Pedidos.ListarPedidoPorNomeCpfCnpj
{
    public class ListarPedidoPorNomeCpfCnpjHandler : Notifiable, IRequestHandler<ListarPedidoPorNomeCpfCnpjRequest, Commands.Response>
    {
        private readonly IRepositoryPedido _repositoryPedido;

        public ListarPedidoPorNomeCpfCnpjHandler(IRepositoryPedido repositoryPedido)
        {
            _repositoryPedido = repositoryPedido;
        }

        public async Task<Commands.Response> Handle(ListarPedidoPorNomeCpfCnpjRequest request, CancellationToken cancellationToken)
        {
            // Valida se o objeto request esta nulo
            if (request == null)
            {
                AddNotification("Erro", "Handle");
                return new Commands.Response(this);
            }

            // Lista pedidos filtrando pelo CNPJ do Cliente
            var collection = await _repositoryPedido.ListarPorAsync(
                x => x.Cliente.CpfCnpj == request.Cnpj,
                cancellationToken,
                x => x.Colaborador,
                x => x.Cliente);

            if (!collection.Any())
            {
                // Correção de mensagens acentuadas
                AddNotification("ATENÇÃO", "Pedido NÃO ENCONTRADO");
                return new Commands.Response(this);
            }

            // Criar meu objeto de resposta
            var response = new Commands.Response(this, collection);

            // Retorna o resultado
            return response;
        }
    }
}
