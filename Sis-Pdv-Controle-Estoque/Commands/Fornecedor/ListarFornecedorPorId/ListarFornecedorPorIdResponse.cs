namespace Commands.Fornecedor.ListarFornecedorPorId
{
    public class ListarFornecedorPorIdResponse
    {
        public Guid? Id { get; set; }
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

        public static explicit operator ListarFornecedorPorIdResponse(Model.Fornecedor request)
        {
            return new ListarFornecedorPorIdResponse()
            {
                InscricaoEstadual = request.InscricaoEstadual,
                NomeFantasia = request.NomeFantasia,
                Uf = request.Uf,
                Numero = request.Numero,
                Complemento = request.Complemento,
                Bairro = request.Bairro,
                Cidade = request.Cidade,
                CepFornecedor = request.CepFornecedor,
                StatusAtivo = request.StatusAtivo,
                Cnpj = request.Cnpj,
                Rua = request.Rua,
                Id = request.Id
            };
        }
    }
}
