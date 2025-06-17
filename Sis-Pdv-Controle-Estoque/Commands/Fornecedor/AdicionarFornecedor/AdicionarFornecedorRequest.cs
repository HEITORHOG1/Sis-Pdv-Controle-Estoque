using MediatR;

namespace Commands.Fornecedor.AdicionarFornecedor
{
    public class AdicionarFornecedorRequest : IRequest<Response>
    {
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
