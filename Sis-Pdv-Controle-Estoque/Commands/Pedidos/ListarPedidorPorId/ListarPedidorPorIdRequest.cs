
using MediatR;
using prmToolkit.NotificationPattern;
using Sis_Pdv_Controle_Estoque.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commands.Pedido.ListarPedidoPorId
{
    public class ListarPedidoPorIdRequest : IRequest<Sis_Pdv_Controle_Estoque.Commands.Response>
    {
        public ListarPedidoPorIdRequest(Guid? id)
        {
            Id = id;
        }

        public Guid? Id { get; set; }
        
    }
}
