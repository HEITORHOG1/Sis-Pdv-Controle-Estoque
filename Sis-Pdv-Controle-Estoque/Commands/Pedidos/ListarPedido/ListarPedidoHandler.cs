using MediatR;
using Microsoft.EntityFrameworkCore;
using prmToolkit.NotificationPattern;

namespace Commands.Pedidos.ListarPedido
{
    public class ListarPedidoPorIdHandler : Notifiable, IRequestHandler<ListarPedidoRequest, Commands.Response>
    {
        private readonly IMediator _mediator;
        private readonly IRepositoryPedido _repositoryPedido;

        public ListarPedidoPorIdHandler(IMediator mediator, IRepositoryPedido repositoryPedido)
        {
            _mediator = mediator;
            _repositoryPedido = repositoryPedido;
        }

        public async Task<Commands.Response> Handle(ListarPedidoRequest request, CancellationToken cancellationToken)
        {
            //Valida se o objeto request esta nulo
            if (request == null)
            {
                AddNotification("Request", "");
                return new Commands.Response(this);
            }

            var grupoCollection = _repositoryPedido.Listar().Include(x => x.Colaborador).ToList();


            //Cria objeto de resposta
            var response = new Commands.Response(this, grupoCollection);

            ////Retorna o resultado
            return await Task.FromResult(response);
        }
    }
}

