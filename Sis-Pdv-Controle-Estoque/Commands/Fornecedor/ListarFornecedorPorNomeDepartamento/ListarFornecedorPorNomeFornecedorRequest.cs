using MediatR;
using prmToolkit.NotificationPattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commands.Fornecedor.ListarFornecedorPorNomeFornecedor
{
    public class ListarFornecedorPorNomeFornecedorRequest : IRequest<ListarFornecedorPorNomeFornecedorResponse>
    {
        public ListarFornecedorPorNomeFornecedorRequest(string Cnpj)
        {
            this.Cnpj = Cnpj;  
        }
        public string Cnpj { get; set; }
    }
}

