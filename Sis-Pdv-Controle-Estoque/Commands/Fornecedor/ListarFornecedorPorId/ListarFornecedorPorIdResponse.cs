using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Commands.Fornecedor.ListarFornecedorPorId
{
    public class ListarFornecedorPorIdResponse
    {
        public Guid? Id { get; set; }
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

        public static explicit operator ListarFornecedorPorIdResponse(Sis_Pdv_Controle_Estoque.Model.Fornecedor request)
        {
            return new ListarFornecedorPorIdResponse()
            {
                inscricaoEstadual = request.inscricaoEstadual,
                nomeFantasia = request.nomeFantasia,
                Uf = request.Uf,
                Numero = request.Numero,
                Complemento = request.Complemento,
                Bairro = request.Bairro,
                Cidade = request.Cidade,
                cepFornecedor = request.cepFornecedor,
                statusAtivo = request.statusAtivo,
                Cnpj = request.Cnpj,
                Rua = request.Rua,
                Id = request.Id
            };
        }
    }
}
