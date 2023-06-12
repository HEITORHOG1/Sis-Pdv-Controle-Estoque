using Commands.Categoria.ListarCategoria.ListarCategoriaPorId;
using MediatR;
using prmToolkit.NotificationPattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commands.Categoria.ListarCategoriaPorId
{
    public class ListarCategoriaPorIdRequest : IRequest<ListarCategoriaPorIdResponse>
    {
        public ListarCategoriaPorIdRequest(Guid id)
        {
            Id = id;
        }
        public Guid Id { get; set; }
    }
}
