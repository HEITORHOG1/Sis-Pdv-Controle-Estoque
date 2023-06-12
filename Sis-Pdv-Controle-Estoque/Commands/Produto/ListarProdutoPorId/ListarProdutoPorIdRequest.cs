
using MediatR;
using prmToolkit.NotificationPattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commands.Produto.ListarProdutoPorId
{
    public class ListarProdutoPorIdRequest : IRequest<ListarProdutoPorIdResponse>
    {
        public ListarProdutoPorIdRequest(Guid id)
        {
            Id = id;
        }
        public Guid Id { get; set; }
    }
}
