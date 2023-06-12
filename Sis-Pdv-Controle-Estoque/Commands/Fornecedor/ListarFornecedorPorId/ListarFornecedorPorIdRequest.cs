
using MediatR;
using prmToolkit.NotificationPattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commands.Fornecedor.ListarFornecedorPorId
{
    public class ListarFornecedorPorIdRequest : IRequest<ListarFornecedorPorIdResponse>
    {
        public ListarFornecedorPorIdRequest(Guid id)
        {
            Id = id;
        }
        public Guid Id { get; set; }
    }
}
