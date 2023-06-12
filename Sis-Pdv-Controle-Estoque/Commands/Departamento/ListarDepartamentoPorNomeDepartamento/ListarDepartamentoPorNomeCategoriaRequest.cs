using MediatR;
using prmToolkit.NotificationPattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commands.Departamento.ListarDepartamentoPorNomeDepartamento
{
    public class ListarDepartamentoPorNomeDepartamentoRequest : IRequest<ListarDepartamentoPorNomeDepartamentoResponse>
    {
        public ListarDepartamentoPorNomeDepartamentoRequest(string NomeDepartamento)
        {
            this.NomeDepartamento = NomeDepartamento;  
        }
        public string NomeDepartamento { get; set; }
    }
}

