using MediatR;
using prmToolkit.NotificationPattern;
using Sis_Pdv_Controle_Estoque.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commands.ProdutoPedido.ListarProdutoPedidoPorNomeCpfCnpj
{
    public class ListarProdutoPedidoPorCodigoDeBarrasRequest : IRequest<Sis_Pdv_Controle_Estoque.Commands.Response>
    {
        public ListarProdutoPedidoPorCodigoDeBarrasRequest(string codBarras)
        {
            CodBarras = codBarras;
        }

        public string CodBarras { get; set; }
    }
}

