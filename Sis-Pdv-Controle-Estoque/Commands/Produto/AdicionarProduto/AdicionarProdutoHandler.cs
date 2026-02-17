using MediatR;
using prmToolkit.NotificationPattern;

namespace Commands.Produto.AdicionarProduto
{
    public class AdicionarProdutoHandler : Notifiable, IRequestHandler<AdicionarProdutoRequest, Response>
    {
        private readonly IRepositoryProduto _repositoryProduto;

        public AdicionarProdutoHandler(IRepositoryProduto repositoryProduto)
        {
            _repositoryProduto = repositoryProduto;
        }

        public Task<Response> Handle(AdicionarProdutoRequest request, CancellationToken cancellationToken)
        {
            // Validation is now handled by the ValidationBehavior pipeline

            //Verificar se o produto j� existe
            if (_repositoryProduto.Existe(x => x.CodBarras == request.CodBarras))
            {
                AddNotification("CodBarras", "Produto j� cadastrado com este c�digo de barras");
                return Task.FromResult(new Response(this));
            }

            Model.Produto Produto = new(
                                request.CodBarras,
                                request.NomeProduto,
                                request.DescricaoProduto,
                                request.PrecoCusto,
                                request.PrecoVenda,
                                request.MargemLucro,
                                request.DataFabricao,
                                request.DataVencimento,
                                request.QuantidadeEstoqueProduto,
                                request.FornecedorId,
                                request.CategoriaId,
                                request.StatusAtivo);

            if (IsInvalid())
            {
                return Task.FromResult(new Response(this));
            }

            Produto = _repositoryProduto.Adicionar(Produto);

            //Criar meu objeto de resposta
            var response = new Response(this, Produto);

            return Task.FromResult(response);
        }
    }
}
