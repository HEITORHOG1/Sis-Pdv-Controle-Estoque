using MediatR;
using Sis_Pdv_Controle_Estoque.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commands.Categoria.AdicionarCliente
{
    public class AdicionarClienteRequest : IRequest<Response>
    {
        public string CpfCnpj { get; set; }
        public string TipoCliente { get; set; }
    }
}
