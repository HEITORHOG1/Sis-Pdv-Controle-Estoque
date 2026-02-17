using MediatR;
using prmToolkit.NotificationPattern;

namespace Commands.Produto.RemoverProduto

{
    public class RemoverProdutoHandler : Notifiable, IRequestHandler<RemoverProdutoRequest, Commands.Response>
    {
        private readonly IRepositoryProduto _repositoryProduto;

        public RemoverProdutoHandler(IRepositoryProduto repositoryProduto)
        {
            _repositoryProduto = repositoryProduto;
        }

        public Task<Commands.Response> Handle(RemoverProdutoRequest request, CancellationToken cancellationToken)
        {
            //Valida se o objeto request esta nulo
            if (request == null)
            {
                AddNotification("Request", "A requisição não pode ser nula.");
                return Task.FromResult(new Commands.Response(this));
            }

            Model.Produto Produto = _repositoryProduto.ObterPorId(request.Id);

            if (Produto == null)
            {
                AddNotification("Request", "A requisição não pode ser nula.");
                return Task.FromResult(new Commands.Response(this));
            }

            _repositoryProduto.Remover(Produto);

            var result = new { Id = Produto.Id };

            //Cria objeto de resposta
            var response = new Commands.Response(this, result);

            ////Retorna o resultado
            return Task.FromResult(response);
        }
    }
}
