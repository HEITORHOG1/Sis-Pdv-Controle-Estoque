using MediatR;
using Sis_Pdv_Controle_Estoque.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commands.Categoria.AdicionarCategoria
{
    public class AdicionarCategoriaRequest : IRequest<Response>
    {
        public string? NomeCategoria { get; set; }
    }
}
