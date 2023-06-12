using MediatR;
using prmToolkit.NotificationPattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commands.Categoria.ListarCategoria.ListarCategoriaPorNomeCategoria
{
    public class ListarCategoriaPorNomeCategoriaRequest : IRequest<ListarCategoriaPorNomeCategoriaResponse>
    {
        public ListarCategoriaPorNomeCategoriaRequest(string nomeCategoria)
        {
            NomeCategoria = nomeCategoria;  
        }
        public string NomeCategoria { get; set; }
    }
}

