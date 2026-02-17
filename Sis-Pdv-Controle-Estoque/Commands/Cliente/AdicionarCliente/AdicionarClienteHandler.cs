using MediatR;
using prmToolkit.NotificationPattern;

namespace Commands.Cliente.AdicionarCliente
{
    public class AdicionarClienteHandler : Notifiable, IRequestHandler<AdicionarClienteRequest, Response>
    {
        private readonly IRepositoryCliente _repositoryCliente;

        public AdicionarClienteHandler(IRepositoryCliente repositoryCliente)
        {
            _repositoryCliente = repositoryCliente;
        }

        public async Task<Response> Handle(AdicionarClienteRequest request, CancellationToken cancellationToken)
        {
            // Validation is now handled by the ValidationBehavior pipeline

            //Verificar se o cliente j� existe
            if (await _repositoryCliente.ExisteAsync(x => x.CpfCnpj == request.CpfCnpj))
            {
                AddNotification("CpfCnpj", "Cliente j� cadastrado com este CPF/CNPJ");
                return new Response(this);
            }

            Model.Cliente cliente = new(request.CpfCnpj, request.TipoCliente);

            if (IsInvalid())
            {
                return new Response(this);
            }
            
            cliente = await _repositoryCliente.AdicionarAsync(cliente, cancellationToken);

            //Criar meu objeto de resposta
            var response = new Response(this, cliente);

            return response;
        }
    }
}
