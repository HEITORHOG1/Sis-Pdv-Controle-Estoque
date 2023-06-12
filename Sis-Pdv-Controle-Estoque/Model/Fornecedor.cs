using Sis_Pdv_Controle_Estoque.Model.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sis_Pdv_Controle_Estoque.Model
{
    [Table("Fornecedor")]
    public  class Fornecedor : EntityBase
    {
        public Fornecedor()
        {
                
        }
        public Fornecedor(string inscricaoEstadual, string nomeFantasia, string uf, string numero, 
            string complemento, string bairro, string cidade, int cepFornecedor, int statusAtivo, string cnpj, string rua)
        {
            this.inscricaoEstadual = inscricaoEstadual;
            this.nomeFantasia = nomeFantasia;
            this.Uf = uf;
            this.Numero = numero;
            this.Complemento = complemento;
            this.Bairro = bairro;
            this.Cidade = cidade;
            this.cepFornecedor = cepFornecedor;
            this.statusAtivo = statusAtivo;
            Cnpj = cnpj;
            Rua = rua;
            Cidade = cidade;
        }
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

        internal void AlterarFornecedor(Guid id, string inscricaoEstadual, string nomeFantasia, string uf, string numero,
            string complemento, string bairro, string cidade, int cepFornecedor, int statusAtivo, string cnpj, string rua)
        {
            this.inscricaoEstadual = inscricaoEstadual;
            this.nomeFantasia = nomeFantasia;
            this.Uf = uf;
            this.Numero = numero;
            this.Complemento = complemento;
            this.Bairro = bairro;
            this.Cidade = cidade;
            this.cepFornecedor = cepFornecedor;
            this.statusAtivo = statusAtivo;
            Cnpj = cnpj;
            Rua = rua;
            Cidade = cidade;
            this.Id = id;
        }
    }
}
