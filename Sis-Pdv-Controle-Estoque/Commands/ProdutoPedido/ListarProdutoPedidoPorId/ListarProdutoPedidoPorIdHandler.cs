using MediatR;
using prmToolkit.NotificationPattern;

namespace Commands.ProdutoPedido.ListarProdutoPedidoPorId
{
    public class ListarProdutoPedidoPorIdHandler : Notifiable, IRequestHandler<ListarProdutoPedidoPorIdRequest, Sis_Pdv_Controle_Estoque.Commands.Response>
    {
        private readonly IMediator _mediator;
        private readonly IRepositoryProdutoPedido _repositoryProdutoPedido;

        public ListarProdutoPedidoPorIdHandler(IMediator mediator, IRepositoryProdutoPedido repositoryProdutoPedido)
        {
            _mediator = mediator;
            _repositoryProdutoPedido = repositoryProdutoPedido;
        }

        public async Task<Sis_Pdv_Controle_Estoque.Commands.Response> Handle(ListarProdutoPedidoPorIdRequest request, CancellationToken cancellationToken)
        {
            //Valida se o objeto request esta nulo
            if (request == null)
            {
                AddNotification("Request", "");
                return new Sis_Pdv_Controle_Estoque.Commands.Response(this);
            }

            var Collection =
                _repositoryProdutoPedido.ListarProdutosPorPedidoId(request.Id);

            if (Collection == null)
            {
                AddNotification("Request", "");
                return new Sis_Pdv_Controle_Estoque.Commands.Response(this);
            }

            //Cria objeto de resposta
            var response = new Sis_Pdv_Controle_Estoque.Commands.Response(this, Collection.Result);

            ////Retorna o resultado
            return await Task.FromResult(response);
        }
    }
}

