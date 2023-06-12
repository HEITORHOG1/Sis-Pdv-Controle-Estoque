using MediatR;
using prmToolkit.NotificationPattern;
using Sis_Pdv_Controle_Estoque.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commands.Pedido.ListarPedidoPorNomeCpfCnpj
{
    public class ListarPedidoPorNomeCpfCnpjRequest : IRequest<Sis_Pdv_Controle_Estoque.Commands.Response>
    {
        public ListarPedidoPorNomeCpfCnpjRequest(string cnpj)
        {
            Cnpj = cnpj;
        }

        public string Cnpj { get; set; }
    }
}

