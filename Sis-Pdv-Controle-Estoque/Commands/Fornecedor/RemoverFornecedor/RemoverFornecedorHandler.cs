using MediatR;
using prmToolkit.NotificationPattern;

namespace Commands.Fornecedor.RemoverFornecedor

{
    public class RemoverFornecedorHandler : Notifiable, IRequestHandler<RemoverFornecedorRequest, Commands.Response>
    {
        private readonly IRepositoryFornecedor _repositoryFornecedor;

        public RemoverFornecedorHandler(IRepositoryFornecedor repositoryFornecedor)
        {
            _repositoryFornecedor = repositoryFornecedor;
        }

        public Task<Commands.Response> Handle(RemoverFornecedorRequest request, CancellationToken cancellationToken)
        {
            //Valida se o objeto request esta nulo
            if (request == null)
            {
                AddNotification("Request", "A requisição não pode ser nula.");
                return Task.FromResult(new Commands.Response(this));
            }

            Model.Fornecedor Fornecedor = _repositoryFornecedor.ObterPorId(request.Id);

            if (Fornecedor == null)
            {
                AddNotification("Request", "A requisição não pode ser nula.");
                return Task.FromResult(new Commands.Response(this));
            }

            _repositoryFornecedor.Remover(Fornecedor);

            var result = new { Id = Fornecedor.Id };

            //Cria objeto de resposta
            var response = new Commands.Response(this, result);

            ////Retorna o resultado
            return Task.FromResult(response);
        }
    }
}
