﻿using MediatR;
using Microsoft.EntityFrameworkCore;
using prmToolkit.NotificationPattern;

namespace Commands.Pedidos.ListarPedidoPorNomeCpfCnpj
{
    public class ListarPedidoPorNomeCpfCnpjHandler : Notifiable, IRequestHandler<ListarPedidoPorNomeCpfCnpjRequest, Commands.Response>
    {
        private readonly IMediator _mediator;
        private readonly IRepositoryPedido _repositoryPedido;

        public ListarPedidoPorNomeCpfCnpjHandler(IMediator mediator, IRepositoryPedido repositoryPedido)
        {
            _mediator = mediator;
            _repositoryPedido = repositoryPedido;
        }

        public async Task<Commands.Response> Handle(ListarPedidoPorNomeCpfCnpjRequest request, CancellationToken cancellationToken)
        {
            //Valida se o objeto request esta nulo
            if (request == null)
            {
                AddNotification("Erro", "Handle");
                return new Commands.Response(this);
            }

            var Collection = _repositoryPedido.Listar()
                                .Include(x => x.Colaborador)
                                .Include(x => x.Cliente)
                                .Where(x => x.Cliente.CpfCnpj == request.Cnpj);
            if (!Collection.Any())
            {
                AddNotification("ATENÇÃO", "Pedido NÃO ENCONTRADA");
                return new Commands.Response(this);
            }

            //Criar meu objeto de resposta
            var response = new Commands.Response(this, Collection);
            //Cria objeto de resposta

            ////Retorna o resultado
            return await Task.FromResult(response);
        }
    }
}


