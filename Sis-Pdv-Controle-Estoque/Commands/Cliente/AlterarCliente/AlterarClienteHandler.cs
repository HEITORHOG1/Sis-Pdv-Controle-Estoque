using MediatR;
using prmToolkit.NotificationPattern;

namespace Commands.Cliente.AlterarCliente
{
    public class AlterarClienteHandler : Notifiable, IRequestHandler<AlterarClienteRequest, Response>
    {
        private readonly IRepositoryCliente _repositoryCliente;

        public AlterarClienteHandler(IRepositoryCliente repositoryCliente)
        {
            _repositoryCliente = repositoryCliente;
        }

        public async Task<Response> Handle(AlterarClienteRequest request, CancellationToken cancellationToken)
        {
            var cliente = await _repositoryCliente.ObterPorIdAsync(request.Id, cancellationToken);

            if (cliente == null)
            {
                AddNotification("Cliente", "Cliente não encontrado.");
                return new Response(this);
            }

            // Verificar se já existe outro cliente com o mesmo CPF/CNPJ
            if (await _repositoryCliente.ExisteAsync(x => x.CpfCnpj == request.CpfCnpj && x.Id != request.Id, cancellationToken))
            {
                AddNotification("CpfCnpj", "Já existe outro cliente cadastrado com este CPF/CNPJ.");
                return new Response(this);
            }

            cliente.AtualizarDados(request.CpfCnpj, request.TipoCliente);

            await _repositoryCliente.EditarAsync(cliente, cancellationToken);

            return new Response(this, cliente);
        }
    }
}
