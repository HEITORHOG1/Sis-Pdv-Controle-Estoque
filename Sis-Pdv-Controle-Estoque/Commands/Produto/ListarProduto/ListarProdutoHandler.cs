using MediatR;
using Microsoft.EntityFrameworkCore;
using prmToolkit.NotificationPattern;

namespace Commands.Produto.ListarProduto
{
    public class ListarProdutoPorIdHandler : Notifiable, IRequestHandler<ListarProdutoRequest, Commands.Response>
    {
        private readonly IMediator _mediator;
        private readonly IRepositoryProduto _repositoryProduto;

        public ListarProdutoPorIdHandler(IMediator mediator, IRepositoryProduto repositoryProduto)
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

            var _produto = _repositoryProduto.Listar().Include(x => x.Fornecedor).Include(x => x.Categoria).ToList();
            List<ListarProdutoRequest> _lista = new List<ListarProdutoRequest>();            foreach (var item in _produto)
            {
                ListarProdutoRequest produto = new ListarProdutoRequest
                {
                    Id = item.Id,
                    codBarras = item.CodBarras,
                    nomeProduto = item.NomeProduto,
                    NomeFornecedor = item.Fornecedor.NomeFantasia,
                    NomeCategoria = item.Categoria.NomeCategoria,
                    descricaoProduto = item.DescricaoProduto,
                    isPerecivel = item.IsPerecivel,
                    statusAtivo = item.StatusAtivo
                };
                _lista.Add(produto);
            }

            //var result = new { Id = _produto., NomeProduto = _produto.NomeProduto};

            //Cria objeto de resposta
            var response = new Commands.Response(this, _lista);

            ////Retorna o resultado
            return await Task.FromResult(response);
        }
    }
}

