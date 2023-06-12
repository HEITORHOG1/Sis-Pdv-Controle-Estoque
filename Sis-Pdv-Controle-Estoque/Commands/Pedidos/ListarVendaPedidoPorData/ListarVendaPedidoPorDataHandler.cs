using MediatR;
using Microsoft.EntityFrameworkCore;
using prmToolkit.NotificationPattern;
using Sis_Pdv_Controle_Estoque.Interfaces;
using System.Text.RegularExpressions;

namespace Commands.Pedido.ListarVendaPedidoPorData
{
    public class ListarVendaPedidoPorDataHandler : Notifiable, IRequestHandler<ListarVendaPedidoPorDataRequest, Sis_Pdv_Controle_Estoque.Commands.Response>
    {
        private readonly IMediator _mediator;
        private readonly IRepositoryPedido _repositoryPedido;

        public ListarVendaPedidoPorDataHandler(IMediator mediator, IRepositoryPedido repositoryPedido)
        {
            _mediator = mediator;
            _repositoryPedido = repositoryPedido;
        }

        public async Task<Sis_Pdv_Controle_Estoque.Commands.Response> Handle(ListarVendaPedidoPorDataRequest request, CancellationToken cancellationToken)
        {
            //Valida se o objeto request esta nulo
            if (request == null)
            {
                AddNotification("Erro", "Handle");
                return new Sis_Pdv_Controle_Estoque.Commands.Response(this);
            }

            var Collection = _repositoryPedido.ListarVendaPedidoPorData(request.DataInicio, request.DataFim);

            if (!Collection.Result.Any()) 
            {
                AddNotification("ATENÇÃO", "Pedido NÃO ENCONTRADA");
                return new Sis_Pdv_Controle_Estoque.Commands.Response(this);
            }

            //Criar meu objeto de resposta
            var response = new Sis_Pdv_Controle_Estoque.Commands.Response(this, Collection.Result);
            //Cria objeto de resposta

            ////Retorna o resultado
            return await Task.FromResult(response);
        }
    }
}


