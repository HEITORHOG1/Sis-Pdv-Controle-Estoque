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

        public static explicit operator ListarFornecedorPorIdResponse(Model.Fornecedor request)
        {
            return new ListarFornecedorPorIdResponse()
            {
                inscricaoEstadual = request.InscricaoEstadual,
                nomeFantasia = request.NomeFantasia,
                Uf = request.Uf,
                Numero = request.Numero,
                Complemento = request.Complemento,
                Bairro = request.Bairro,
                Cidade = request.Cidade,
                cepFornecedor = request.CepFornecedor,
                statusAtivo = request.StatusAtivo,
                Cnpj = request.Cnpj,
                Rua = request.Rua,
                Id = request.Id
            };
        }
    }
}
