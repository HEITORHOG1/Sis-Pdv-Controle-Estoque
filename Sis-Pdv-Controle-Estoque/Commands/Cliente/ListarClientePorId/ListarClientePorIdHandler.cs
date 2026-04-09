using Interfaces;
using MediatR;
using prmToolkit.NotificationPattern;

namespace Commands.Cliente.ListarClientePorId
{
    public class ListarClientePorIdHandler : Notifiable, IRequestHandler<ListarClientePorIdRequest, Response>
    {
        private readonly IRepositoryCliente _repositoryCliente;

        public ListarClientePorIdHandler(IRepositoryCliente repositoryCliente)
        {
            _repositoryCliente = repositoryCliente;
        }

        public Task<Response> Handle(ListarClientePorIdRequest request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                AddNotification("Request", "A requisição não pode ser nula.");
                return Task.FromResult(new Response(this));
            }

            var cliente = _repositoryCliente.ObterPorId(request.Id);

            if (cliente == null)
            {
                AddNotification("Cliente", "Cliente não encontrado.");
                return Task.FromResult(new Response(this));
            }

            var response = new Response(this, cliente);
            return Task.FromResult(response);
        }
    }
}
