using MediatR;
using prmToolkit.NotificationPattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commands.Produto.ListarProdutoPorCodBarras
{
    public class ListarProdutoPorCodBarrasRequest : IRequest<Sis_Pdv_Controle_Estoque.Commands.Response>
    {
        public ListarProdutoPorCodBarrasRequest(string codBarras)
        {
            this.codBarras = codBarras;  
        }
        public string codBarras { get; set; }
    }
}

