using MediatR;
using prmToolkit.NotificationPattern;

namespace Commands.Produto.AlterarProduto
{
    public class AlterarProdutoHandler : Notifiable, IRequestHandler<AlterarProdutoRequest, Response>
    {
        private readonly IRepositoryProduto _repositoryProduto;

        public AlterarProdutoHandler(IRepositoryProduto repositoryProduto)
        {
            _repositoryProduto = repositoryProduto;
        }

        public async Task<Response> Handle(AlterarProdutoRequest request, CancellationToken cancellationToken)
        {

            Model.Produto Produto = new Model.Produto();

            Produto.AlterarProduto(
                request.Id,
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

            var retornoExist = _repositoryProduto.Listar().Where(x => x.Id == request.Id);

            if (!retornoExist.Any())
            {
                AddNotification("Produto", "retornoExist");
                return new Response(this);
            }

            Produto = await _repositoryProduto.EditarAsync(Produto, cancellationToken);

            //Criar meu objeto de resposta
            var response = new Response(this, Produto);

            return response;
        }
    }
}

