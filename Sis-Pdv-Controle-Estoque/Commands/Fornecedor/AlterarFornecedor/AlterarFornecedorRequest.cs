using MediatR;

namespace Commands.Fornecedor.AlterarFornecedor
{
    public class AlterarFornecedorRequest : IRequest<Response>
    {
        public Guid Id { get; set; }
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
    }
}
