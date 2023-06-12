using MediatR;
using prmToolkit.NotificationPattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sis_Pdv_Controle_Estoque.Commands.Colaborador.ListarColaboradorPorNomeColaborador
{
    public class ListarColaboradorPorNomeColaboradorRequest : IRequest<ListarColaboradorPorNomeColaboradorResponse>
    {
        public ListarColaboradorPorNomeColaboradorRequest(string NomeColaborador)
        {
            this.NomeColaborador = NomeColaborador;  
        }
        public string NomeColaborador { get; set; }
    }
}

