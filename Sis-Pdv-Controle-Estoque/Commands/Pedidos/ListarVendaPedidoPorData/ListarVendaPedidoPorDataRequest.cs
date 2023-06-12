using MediatR;
using prmToolkit.NotificationPattern;
using Sis_Pdv_Controle_Estoque.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commands.Pedido.ListarVendaPedidoPorData
{
    public class ListarVendaPedidoPorDataRequest : IRequest<Sis_Pdv_Controle_Estoque.Commands.Response>
    {
        public ListarVendaPedidoPorDataRequest(DateTime dataInicio, DateTime dataFim)
        {
            DataInicio = dataInicio;
            DataFim = dataFim;  
        }

        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
    }
}

