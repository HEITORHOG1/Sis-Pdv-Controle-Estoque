using MediatR;
using Sis_Pdv_Controle_Estoque.Commands;
using Sis_Pdv_Controle_Estoque.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commands.Pedido.AdicionarPedido
{
    public class AdicionarPedidoRequest : IRequest<Sis_Pdv_Controle_Estoque.Commands.Response>
    {
        public Guid Id { get; set; }
        public Guid ColaboradorId { get; set; }
        public Guid? ClienteId { get; set; }
        public int Status { get; set; }
        public DateTime dataDoPedido { get; set; }
        public string formaPagamento { get; set; }
        public decimal totalPedido { get; set; }
    }
}
