using MediatR;
using Sis_Pdv_Controle_Estoque.Commands;
using System;

namespace Commands.ProdutoPedido.RemoverProdutoPedido
{
    public class RemoverProdutoPedidoResquest : IRequest<Response>
    {
        public RemoverProdutoPedidoResquest(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}
