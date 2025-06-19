using Interfaces;
using MediatR;
using prmToolkit.NotificationPattern;

namespace Commands.Cliente.ListarClientePorCpfCnpj
{
    public class ListarClientePorCpfCnpjHandler : Notifiable, IRequestHandler<ListarClientePorCpfCnpjRequest, Commands.Response>
    {
        private readonly IMediator _mediator;
        private readonly IRepositoryCliente _repositoryCliente;

        public ListarClientePorCpfCnpjHandler(IMediator mediator, IRepositoryCliente repositoryCliente)
        {
            _mediator = mediator;
            _repositoryCliente = repositoryCliente;
        }

        public async Task<Commands.Response> Handle(ListarClientePorCpfCnpjRequest request, CancellationToken cancellationToken)
        {
            //Valida se o objeto request esta nulo
            if (request == null)
            {
                AddNotification("Erro", "Handle");
                return new Commands.Response(this);
            }

            var Collection = _repositoryCliente.Listar().Where(x => x.CpfCnpj == request.CpfCnpj);
            if (!Collection.Any())
            {
                AddNotification("ATENÇÃO", "CATEGORIA NÃO ENCONTRADA");
                return new Commands.Response(this);
            }

            //Criar meu objeto de resposta
            var response = new Commands.Response(this, Collection);
            //Cria objeto de resposta

            ////Retorna o resultado
            return await Task.FromResult(response);
        }
    }
}


