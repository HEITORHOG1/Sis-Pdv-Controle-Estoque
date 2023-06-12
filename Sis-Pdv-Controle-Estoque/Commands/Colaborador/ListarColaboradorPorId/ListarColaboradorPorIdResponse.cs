using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sis_Pdv_Controle_Estoque.Commands.Colaborador.ListarColaboradorPorId
{
    public class ListarColaboradorPorIdResponse
    {
        public Guid? Id { get; set; }
        public string NomeColaborador { get; set; }

        public static explicit operator ListarColaboradorPorIdResponse(Sis_Pdv_Controle_Estoque.Model.Colaborador cat)
        {
            return new ListarColaboradorPorIdResponse()
            {
                Id = cat.Id,
                NomeColaborador = cat.nomeColaborador
            };
        }
    }
}
