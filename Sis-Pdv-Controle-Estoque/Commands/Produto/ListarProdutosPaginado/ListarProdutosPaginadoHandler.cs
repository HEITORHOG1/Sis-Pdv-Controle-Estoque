using MediatR;
using prmToolkit.NotificationPattern;
using Model;
using System.Linq.Expressions;

namespace Commands.Produto.ListarProdutosPaginado
{
    public class ListarProdutosPaginadoHandler : Notifiable, IRequestHandler<ListarProdutosPaginadoRequest, Response>
    {
        private readonly IRepositoryProduto _repositoryProduto;

        public ListarProdutosPaginadoHandler(IRepositoryProduto repositoryProduto)
        {
            _repositoryProduto = repositoryProduto;
        }

        public async Task<Response> Handle(ListarProdutosPaginadoRequest request, CancellationToken cancellationToken)
        {
            try
            {
                // Build the where expression based on filters
                Expression<Func<Model.Produto, bool>>? whereExpression = null;

                if (!string.IsNullOrEmpty(request.SearchTerm))
                {
                    whereExpression = p => p.NomeProduto.Contains(request.SearchTerm) || 
                                          p.CodBarras.Contains(request.SearchTerm);
                }

                if (!string.IsNullOrEmpty(request.Categoria))
                {
                    var categoriaFilter = whereExpression;
                    whereExpression = p => (categoriaFilter == null || categoriaFilter.Compile()(p)) &&
                                          p.Categoria.NomeCategoria.Contains(request.Categoria);
                }

                if (request.PrecoMinimo.HasValue)
                {
                    var precoMinimoFilter = whereExpression;
                    whereExpression = p => (precoMinimoFilter == null || precoMinimoFilter.Compile()(p)) &&
                                          p.PrecoVenda >= request.PrecoMinimo.Value;
                }

                if (request.PrecoMaximo.HasValue)
                {
                    var precoMaximoFilter = whereExpression;
                    whereExpression = p => (precoMaximoFilter == null || precoMaximoFilter.Compile()(p)) &&
                                          p.PrecoVenda <= request.PrecoMaximo.Value;
                }

                if (request.ApenasAtivos == true)
                {
                    var ativosFilter = whereExpression;
                    whereExpression = p => (ativosFilter == null || ativosFilter.Compile()(p)) &&
                                          !p.IsDeleted;
                }

                // Get total count
                var totalCount = await _repositoryProduto.ContarAsync(whereExpression);

                // Get paginated results
                var produtos = await _repositoryProduto.ListarPaginadoAsync(
                    request.PageNumber, 
                    request.PageSize, 
                    whereExpression,
                    p => p.Categoria);

                // Create paginated result
                var pagedResult = new PagedResult<Model.Produto>(
                    produtos, 
                    totalCount, 
                    request.PageNumber, 
                    request.PageSize);

                return new Response(this, pagedResult);
            }
            catch (Exception ex)
            {
                AddNotification("Error", $"Erro ao listar produtos paginados: {ex.Message}");
                return new Response(this);
            }
        }
    }
}