using MediatR;
using prmToolkit.NotificationPattern;

namespace Commands.Produto.AdicionarProduto
{
    public class AdicionarProdutoHandler : Notifiable, IRequestHandler<AdicionarProdutoRequest, Response>
    {
        private readonly IMediator _mediator;
        private readonly IRepositoryProduto _repositoryProduto;

        public AdicionarProdutoHandler(IMediator mediator, IRepositoryProduto repositoryProduto)
        {
            _mediator = mediator;
            _repositoryProduto = repositoryProduto;
        }

        public async Task<Response> Handle(AdicionarProdutoRequest request, CancellationToken cancellationToken)
        {
            // Validation is now handled by the ValidationBehavior pipeline

            //Verificar se o produto já existe
            if (_repositoryProduto.Existe(x => x.CodBarras == request.codBarras))
            {
                AddNotification("codBarras", "Produto já cadastrado com este código de barras");
                return new Response(this);
            }

            Model.Produto Produto = new(
                                request.codBarras,
                                request.nomeProduto,
                                request.descricaoProduto,
                                request.precoCusto,
                                request.precoVenda,
                                request.margemLucro,
                                request.dataFabricao,
                                request.dataVencimento,
                                request.quatidadeEstoqueProduto,
                                request.FornecedorId,
                                request.CategoriaId,
                                request.statusAtivo);

            if (IsInvalid())
            {
                return new Response(this);
            }

            Produto = _repositoryProduto.Adicionar(Produto);

            //Criar meu objeto de resposta
            var response = new Response(this, Produto);

            return await Task.FromResult(response);
        }
    }
}
