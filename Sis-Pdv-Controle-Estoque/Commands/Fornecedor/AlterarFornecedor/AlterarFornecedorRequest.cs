using MediatR;
using Sis_Pdv_Controle_Estoque.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commands.Fornecedor.AlterarFornecedor
{
    public class AlterarFornecedorRequest : IRequest<Response>
    {
        public Guid Id { get; set; }
        public string inscricaoEstadual { get; set; }
        public string nomeFantasia { get; set; }
        public string Uf { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public int cepFornecedor { get; set; }
        public int statusAtivo { get; set; }
        public string Cnpj { get; set; }
        public string Rua { get; set; }
    }
}
