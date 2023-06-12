using MediatR;
using Sis_Pdv_Controle_Estoque.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commands.Departamento.AlterarDepartamento
{
    public class AlterarDepartamentoRequest : IRequest<Sis_Pdv_Controle_Estoque.Commands.Response>
    {
        public Guid Id { get; set; }
        public string NomeDepartamento { get; set; }
    }
}
