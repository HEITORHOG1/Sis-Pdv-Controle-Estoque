using MediatR;
using prmToolkit.NotificationPattern;

namespace Commands.Fornecedor.ListarFornecedor
{
    public class ListarFornecedorPorIdHandler : Notifiable, IRequestHandler<ListarFornecedorRequest, Commands.Response>
    {
        private readonly IMediator _mediator;
        private readonly IRepositoryFornecedor _repositoryFornecedor;

        public ListarFornecedorPorIdHandler(IMediator mediator, IRepositoryFornecedor repositoryFornecedor)
        {
            _mediator = mediator;
            _repositoryFornecedor = repositoryFornecedor;
        }

        public async Task<Commands.Response> Handle(ListarFornecedorRequest request, CancellationToken cancellationToken)
        {
            //Valida se o objeto request esta nulo
            if (request == null)
            {
                AddNotification("Request", "");
                return new Commands.Response(this);
            }

            var grupoCollection = _repositoryFornecedor.Listar().ToList();


            //Cria objeto de resposta
            var response = new Commands.Response(this, grupoCollection);

            ////Retorna o resultado
            return await Task.FromResult(response);
        }
    }
}

