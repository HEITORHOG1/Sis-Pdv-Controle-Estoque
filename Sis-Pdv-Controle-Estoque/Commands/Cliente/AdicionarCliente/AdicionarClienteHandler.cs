using MediatR;
using prmToolkit.NotificationPattern;

namespace Commands.Cliente.AdicionarCliente
{
    public class AdicionarClienteHandler : Notifiable, IRequestHandler<AdicionarClienteRequest, Response>
    {
        private readonly IMediator _mediator;
        private readonly IRepositoryCliente _repositoryCliente;

        public AdicionarClienteHandler(IMediator mediator, IRepositoryCliente repositoryCliente)
        {
            _mediator = mediator;
            _repositoryCliente = repositoryCliente;
        }

        public async Task<Response> Handle(AdicionarClienteRequest request, CancellationToken cancellationToken)
        {
            // Validation is now handled by the ValidationBehavior pipeline

            //Verificar se o cliente já existe
            if (_repositoryCliente.Existe(x => x.CpfCnpj == request.CpfCnpj))
            {
                AddNotification("CpfCnpj", "Cliente já cadastrado com este CPF/CNPJ");
                return new Response(this);
            }

            Model.Cliente cliente = new(request.CpfCnpj, request.TipoCliente);

            if (IsInvalid())
            {
                return new Response(this);
            }
            
            cliente = _repositoryCliente.Adicionar(cliente);

            //Criar meu objeto de resposta
            var response = new Response(this, cliente);

            return await Task.FromResult(response);
        }
    }
}
