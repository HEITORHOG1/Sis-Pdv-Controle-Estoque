using Interfaces;
using MediatR;
using prmToolkit.NotificationPattern;

namespace Commands.Cliente.ListarClientes
{
    public class ListarClientesHandler : Notifiable, IRequestHandler<ListarClientesRequest, Response>
    {
        private readonly IRepositoryCliente _repositoryCliente;

        public ListarClientesHandler(IRepositoryCliente repositoryCliente)
        {
            _repositoryCliente = repositoryCliente;
        }

        public Task<Response> Handle(ListarClientesRequest request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                AddNotification("Request", "A requisição não pode ser nula.");
                return Task.FromResult(new Response(this));
            }

            var clientes = _repositoryCliente.Listar().ToList();

            var response = new Response(this, clientes);
            return Task.FromResult(response);
        }
    }
}
