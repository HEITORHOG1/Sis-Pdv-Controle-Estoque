using MediatR;
using prmToolkit.NotificationPattern;
using Model;
using System.Linq.Expressions;

namespace Commands.Cliente.ListarClientesPaginado
{
    public class ListarClientesPaginadoHandler : Notifiable, IRequestHandler<ListarClientesPaginadoRequest, Response>
    {
        private readonly IRepositoryCliente _repositoryCliente;

        public ListarClientesPaginadoHandler(IRepositoryCliente repositoryCliente)
        {
            _repositoryCliente = repositoryCliente;
        }

        public async Task<Response> Handle(ListarClientesPaginadoRequest request, CancellationToken cancellationToken)
        {
            try
            {
                // Build the where expression based on filters
                Expression<Func<Model.Cliente, bool>>? whereExpression = null;

                if (!string.IsNullOrEmpty(request.SearchTerm))
                {
                    whereExpression = c => c.CpfCnpj.Contains(request.SearchTerm);
                }

                if (!string.IsNullOrEmpty(request.TipoCliente))
                {
                    var tipoFilter = whereExpression;
                    whereExpression = c => (tipoFilter == null || tipoFilter.Compile()(c)) &&
                                          c.TipoCliente.ToString().Contains(request.TipoCliente);
                }

                if (request.ApenasAtivos == true)
                {
                    var ativosFilter = whereExpression;
                    whereExpression = c => (ativosFilter == null || ativosFilter.Compile()(c)) &&
                                          !c.IsDeleted;
                }

                // Get total count
                var totalCount = await _repositoryCliente.ContarAsync(whereExpression);

                // Get paginated results
                var clientes = await _repositoryCliente.ListarPaginadoAsync(
                    request.PageNumber, 
                    request.PageSize, 
                    whereExpression);

                // Create paginated result
                var pagedResult = new PagedResult<Model.Cliente>(
                    clientes, 
                    totalCount, 
                    request.PageNumber, 
                    request.PageSize);

                return new Response(this, pagedResult);
            }
            catch (Exception ex)
            {
                AddNotification("Error", $"Erro ao listar clientes paginados: {ex.Message}");
                return new Response(this);
            }
        }
    }
}