using MediatR;
using prmToolkit.NotificationPattern;

namespace Commands.Produto.RemoverProduto

{
    public class RemoverProdutoHandler : Notifiable, IRequestHandler<RemoverProdutoResquest, Commands.Response>
    {
        private readonly IMediator _mediator;
        private readonly IRepositoryProduto _repositoryProduto;

        public RemoverProdutoHandler(IMediator mediator, IRepositoryProduto repositoryProduto)
        {
            _mediator = mediator;
            _repositoryProduto = repositoryProduto;
        }

        public async Task<Commands.Response> Handle(RemoverProdutoResquest request, CancellationToken cancellationToken)
        {
            //Valida se o objeto request esta nulo
            if (request == null)
            {
                AddNotification("Request", "");
                return new Commands.Response(this);
            }

            Model.Produto Produto = _repositoryProduto.ObterPorId(request.Id);

            if (Produto == null)
            {
                AddNotification("Request", "");
                return new Commands.Response(this);
            }

            _repositoryProduto.Remover(Produto);

            var result = new { Id = Produto.Id };

            //Cria objeto de resposta
            var response = new Commands.Response(this, result);

            ////Retorna o resultado
            return await Task.FromResult(response);
        }
    }
}
