using MediatR;

using prmToolkit.NotificationPattern;

namespace Commands.Produto.ListarProduto
{
    public class ListarProdutoPorIdHandler : Notifiable, IRequestHandler<ListarProdutoRequest, Commands.Response>
    {
        private readonly IRepositoryProduto _repositoryProduto;

        public ListarProdutoPorIdHandler(IRepositoryProduto repositoryProduto)
        {
            _repositoryProduto = repositoryProduto;
        }

        public async Task<Commands.Response> Handle(ListarProdutoRequest request, CancellationToken cancellationToken)
        {
            //Valida se o objeto request esta nulo
            if (request == null)
            {
                AddNotification("Request", "A requisi��o n�o pode ser nula.");
                return new Commands.Response(this);
            }

            var _produto = await _repositoryProduto.ListarAsync(cancellationToken, x => x.Fornecedor, x => x.Categoria);
            List<ListarProdutoRequest> _lista = new List<ListarProdutoRequest>();
            foreach (var item in _produto)
            {
                ListarProdutoRequest produto = new ListarProdutoRequest
                {
                    Id = item.Id,
                    CodBarras = item.CodBarras,
                    NomeProduto = item.NomeProduto,
                    NomeFornecedor = item.Fornecedor.NomeFantasia,
                    NomeCategoria = item.Categoria.NomeCategoria,
                    DescricaoProduto = item.DescricaoProduto,
                    QuantidadeEstoqueProduto = item.QuantidadeEstoqueProduto,
                    PrecoVenda = item.PrecoVenda,
                    PrecoCusto = item.PrecoCusto,
                    MargemLucro = item.MargemLucro,
                    DataFabricao = item.DataFabricao,
                    DataVencimento = item.DataVencimento,
                    StatusAtivo = item.StatusAtivo
                };
                _lista.Add(produto);
            }

            //var result = new { Id = _produto., NomeProduto = _produto.NomeProduto};

            //Cria objeto de resposta
            var response = new Commands.Response(this, _lista);

            ////Retorna o resultado
            return response;
        }
    }
}

