using MediatR;
using Sis_Pdv_Controle_Estoque.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commands.Categoria.AlterarCategoria
{
    public class AlterarCategoriaRequest : IRequest<Response>
    {
        public Guid Id { get; set; }
        public string NomeCategoria { get; set; }
    }
}
