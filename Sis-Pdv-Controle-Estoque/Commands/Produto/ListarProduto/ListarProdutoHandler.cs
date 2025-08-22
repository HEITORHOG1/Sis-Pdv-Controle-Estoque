using MediatR;
using Microsoft.EntityFrameworkCore;
using prmToolkit.NotificationPattern;

namespace Commands.Produto.ListarProduto
{
    // Handler para listar produtos (legado) usado pelo cliente WinForms
    public class ListarProdutoHandler : Notifiable, IRequestHandler<ListarProdutoRequest, Commands.Response>
    {
        private readonly IMediator _mediator;
        private readonly IRepositoryProduto _repositoryProduto;

        public ListarProdutoHandler(IMediator mediator, IRepositoryProduto repositoryProduto)
        {
            _mediator = mediator;
            _repositoryProduto = repositoryProduto;
        }

        public async Task<Commands.Response> Handle(ListarProdutoRequest request, CancellationToken cancellationToken)
        {
            //Valida se o objeto request esta nulo
            if (request == null)
            {
                AddNotification("Request", "");
                return new Commands.Response(this);
            }

            // Carrega produtos incluindo relacionamentos e trata possíveis nulos para evitar exceções
            var produtos = await _repositoryProduto
                .Listar()
                .Include(x => x.Fornecedor)
                .Include(x => x.Categoria)
                .ToListAsync(cancellationToken);

            var lista = new List<ListarProdutoRequest>();

            foreach (var item in produtos)
            {
                // Protege contra relacionamentos eventualmente nulos (dados incompletos)
                var nomeFornecedor = item.Fornecedor?.NomeFantasia ?? string.Empty;
                var nomeCategoria = item.Categoria?.NomeCategoria ?? string.Empty;

                var produto = new ListarProdutoRequest
                {
                    Id = item.Id,
                    codBarras = item.CodBarras,
                    nomeProduto = item.NomeProduto,
                    NomeFornecedor = nomeFornecedor,
                    NomeCategoria = nomeCategoria,
                    descricaoProduto = item.DescricaoProduto,
                    isPerecivel = item.IsPerecivel,
                    statusAtivo = item.StatusAtivo
                };
                lista.Add(produto);
            }

            //var result = new { Id = _produto., NomeProduto = _produto.NomeProduto};

            //Cria objeto de resposta
            var response = new Commands.Response(this, lista);

            ////Retorna o resultado
            return await Task.FromResult(response);
        }
    }
}

