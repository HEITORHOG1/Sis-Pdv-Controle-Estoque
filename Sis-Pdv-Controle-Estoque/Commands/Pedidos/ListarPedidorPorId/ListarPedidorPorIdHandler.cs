using MediatR;
using prmToolkit.NotificationPattern;

namespace Commands.Pedidos.ListarPedidorPorId
{
    public class ListarPedidoPorIdHandler : Notifiable, IRequestHandler<ListarPedidoPorIdRequest, Sis_Pdv_Controle_Estoque.Commands.Response>
    {
        private readonly IMediator _mediator;
        private readonly IRepositoryPedido _repositoryPedido;

        public ListarPedidoPorIdHandler(IMediator mediator, IRepositoryPedido repositoryPedido)
        {
            _mediator = mediator;
            _repositoryPedido = repositoryPedido;
        }

        public async Task<Sis_Pdv_Controle_Estoque.Commands.Response> Handle(ListarPedidoPorIdRequest request, CancellationToken cancellationToken)
        {
            //Valida se o objeto request esta nulo
            if (request == null)
            {
                AddNotification("Request", "");
                return new Sis_Pdv_Controle_Estoque.Commands.Response(this);
            }

            Sis_Pdv_Controle_Estoque.Model.Pedido Collection = _repositoryPedido.ListarPor(x => x.Id == request.Id).FirstOrDefault();

            if (Collection == null)
            {
                AddNotification("Request", "");
                return new Sis_Pdv_Controle_Estoque.Commands.Response(this);
            }

            //Cria objeto de resposta
            var response = new Sis_Pdv_Controle_Estoque.Commands.Response(this, Collection);

            ////Retorna o resultado
            return await Task.FromResult(response);
        }
    }
}

