
using MediatR;
using prmToolkit.NotificationPattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sis_Pdv_Controle_Estoque.Commands.Colaborador.ListarColaboradorPorId
{
    public class ListarColaboradorPorIdRequest : IRequest<ListarColaboradorPorIdResponse>
    {
        public ListarColaboradorPorIdRequest(Guid id)
        {
            Id = id;
        }
        public Guid Id { get; set; }
    }
}
