using MediatR;
using prmToolkit.NotificationPattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commands.Categoria.ListarCategoria.ListarClientePorCpfCnpj
{
    public class ListarClientePorCpfCnpjRequest : IRequest<Sis_Pdv_Controle_Estoque.Commands.Response>
    {
        public ListarClientePorCpfCnpjRequest(string cpfCnpj)
        {
            CpfCnpj = cpfCnpj;
        }

        public string CpfCnpj { get; set; }
    }
}

