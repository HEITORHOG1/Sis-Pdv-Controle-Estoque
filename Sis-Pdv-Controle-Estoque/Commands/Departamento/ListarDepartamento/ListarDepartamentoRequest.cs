using MediatR;
using prmToolkit.NotificationPattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commands.Departamento.ListarDepartamento
{
    public class ListarDepartamentoRequest : IRequest<Sis_Pdv_Controle_Estoque.Commands.Response>
    {
    }
}
