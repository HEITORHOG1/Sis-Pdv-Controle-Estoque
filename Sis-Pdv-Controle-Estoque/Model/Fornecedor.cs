using Model.Exceptions;

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
            ValidarNomeFantasia(nomeFantasia);
            ValidarCnpj(cnpj);

            InscricaoEstadual = inscricaoEstadual;
            NomeFantasia = nomeFantasia;
            Uf = uf;
            Numero = numero;
            Complemento = complemento;
            Bairro = bairro;
            Cidade = cidade;
            CepFornecedor = cepFornecedor;
            StatusAtivo = statusAtivo;
            Cnpj = cnpj;
            Rua = rua;
        }
        public string InscricaoEstadual { get; set; }
        public string NomeFantasia { get; set; }
        public string Uf { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public int CepFornecedor { get; set; }
        public int StatusAtivo { get; set; }
        public string Cnpj { get; set; }
        public string Rua { get; set; }

        internal void AlterarFornecedor(Guid id, string inscricaoEstadual, string nomeFantasia, string uf, string numero,
            string complemento, string bairro, string cidade, int cepFornecedor, int statusAtivo, string cnpj, string rua)
        {
            ValidarNomeFantasia(nomeFantasia);
            ValidarCnpj(cnpj);

            InscricaoEstadual = inscricaoEstadual;
            NomeFantasia = nomeFantasia;
            Uf = uf;
            Numero = numero;
            Complemento = complemento;
            Bairro = bairro;
            Cidade = cidade;
            CepFornecedor = cepFornecedor;
            StatusAtivo = statusAtivo;
            Cnpj = cnpj;
            Rua = rua;
            Id = id;
        }

        private static void ValidarNomeFantasia(string nomeFantasia)
        {
            if (string.IsNullOrWhiteSpace(nomeFantasia))
                throw new DomainException("O nome fantasia do fornecedor é obrigatório.");
        }

        private static void ValidarCnpj(string cnpj)
        {
            if (string.IsNullOrWhiteSpace(cnpj))
                throw new DomainException("O CNPJ do fornecedor é obrigatório.");
        }
    }
}
