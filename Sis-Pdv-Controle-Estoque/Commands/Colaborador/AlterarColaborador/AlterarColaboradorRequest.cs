using MediatR;
using Sis_Pdv_Controle_Estoque.Commands;
using Sis_Pdv_Controle_Estoque.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sis_Pdv_Controle_Estoque.Commands.Colaborador.AlterarColaborador
{
    public class AlterarColaboradorRequest : IRequest<Sis_Pdv_Controle_Estoque.Commands.Response>
    {
        public AlterarColaboradorRequest()
        {
            Usuario = new Usuario();
        }
        public Guid Id { get; set; }
        public string nomeColaborador { get; set; }
        public Guid DepartamentoId { get; set; }
        public string cpfColaborador { get; set; }
        public string cargoColaborador { get; set; }
        public string telefoneColaborador { get; set; }
        public string emailPessoalColaborador { get; set; }
        public string emailCorporativo { get; set; }
        public Usuario Usuario { get; set; }
    }
}
