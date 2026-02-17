using Interfaces;
using MediatR;
using prmToolkit.NotificationPattern;

namespace Commands.Cliente.ListarClientePorCpfCnpj
{
    public class ListarClientePorCpfCnpjHandler : Notifiable, IRequestHandler<ListarClientePorCpfCnpjRequest, Commands.Response>
    {
        private readonly IRepositoryCliente _repositoryCliente;

        public ListarClientePorCpfCnpjHandler(IRepositoryCliente repositoryCliente)
        {
            _repositoryCliente = repositoryCliente;
        }

        public Task<Commands.Response> Handle(ListarClientePorCpfCnpjRequest request, CancellationToken cancellationToken)
        {
            //Valida se o objeto request esta nulo
            if (request == null)
            {
                AddNotification("Erro", "Handle");
                return Task.FromResult(new Commands.Response(this));
            }

            var Collection = _repositoryCliente.Listar().Where(x => x.CpfCnpj == request.CpfCnpj);
            if (!Collection.Any())
            {
                AddNotification("ATENÇÃO", "CATEGORIA NÃO ENCONTRADA");
                return Task.FromResult(new Commands.Response(this));
            }

            //Criar meu objeto de resposta
            var response = new Commands.Response(this, Collection);
            //Cria objeto de resposta

            ////Retorna o resultado
            return Task.FromResult(response);
        }
    }
}


