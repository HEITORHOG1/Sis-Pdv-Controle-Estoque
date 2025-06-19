namespace Model
{
    public class Fornecedor : EntityBase
    {
        public Fornecedor()
        {

        }
        public Fornecedor(string inscricaoEstadual, string nomeFantasia, string uf, string numero,
            string complemento, string bairro, string cidade, int cepFornecedor, int statusAtivo, string cnpj, string rua)
        {
            this.inscricaoEstadual = inscricaoEstadual;
            this.nomeFantasia = nomeFantasia;
            Uf = uf;
            Numero = numero;
            Complemento = complemento;
            Bairro = bairro;
            Cidade = cidade;
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
            Uf = uf;
            Numero = numero;
            Complemento = complemento;
            Bairro = bairro;
            Cidade = cidade;
            this.cepFornecedor = cepFornecedor;
            this.statusAtivo = statusAtivo;
            Cnpj = cnpj;
            Rua = rua;
            Cidade = cidade;
            Id = id;
        }
    }
}
