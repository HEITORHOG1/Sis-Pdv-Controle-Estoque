using MediatR;
using prmToolkit.NotificationPattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commands.Pedido.ListarPedido
{
    public class ListarPedidoRequest : IRequest<Sis_Pdv_Controle_Estoque.Commands.Response>
    {
    }
}
