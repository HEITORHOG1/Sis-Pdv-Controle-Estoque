using MediatR;
using Sis_Pdv_Controle_Estoque.Commands;
using System;

namespace Commands.Pedido.RemoverPedido
{
    public class RemoverPedidoResquest : IRequest<Response>
    {
        public RemoverPedidoResquest(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}
