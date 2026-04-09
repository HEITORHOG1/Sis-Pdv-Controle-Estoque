using MediatR;
using prmToolkit.NotificationPattern;

namespace Commands.Cliente.RemoverCliente
{
    public class RemoverClienteHandler : Notifiable, IRequestHandler<RemoverClienteRequest, Response>
    {
        private readonly IRepositoryCliente _repositoryCliente;

        public RemoverClienteHandler(IRepositoryCliente repositoryCliente)
        {
            _repositoryCliente = repositoryCliente;
        }

        public Task<Response> Handle(RemoverClienteRequest request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                AddNotification("Request", "Request não pode ser nulo.");
                return Task.FromResult(new Response(this));
            }

            var cliente = _repositoryCliente.ObterPorId(request.Id);

            if (cliente == null)
            {
                AddNotification("Cliente", "Cliente não encontrado.");
                return Task.FromResult(new Response(this));
            }

            if (cliente.IsDeleted)
            {
                AddNotification("Cliente", "Cliente já foi removido.");
                return Task.FromResult(new Response(this));
            }

            _repositoryCliente.Remover(cliente);

            var response = new Response(this, cliente);
            return Task.FromResult(response);
        }
    }
}
