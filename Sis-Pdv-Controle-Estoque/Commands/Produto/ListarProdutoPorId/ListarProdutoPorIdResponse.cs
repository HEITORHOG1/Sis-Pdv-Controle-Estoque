namespace Commands.Produto.ListarProdutoPorId
{
    public class ListarProdutoPorIdResponse
    {
        public Guid? Id { get; set; }
        public string CodBarras { get; set; }
        public string NomeProduto { get; set; }
        public string DescricaoProduto { get; set; }
        public decimal PrecoCusto { get; set; }
        public decimal PrecoVenda { get; set; }
        public decimal MargemLucro { get; set; }
        public DateTime DataFabricao { get; set; }
        public DateTime DataVencimento { get; set; }
        public int QuantidadeEstoqueProduto { get; set; }
        public Model.Fornecedor Fornecedor { get; set; }
        public Model.Categoria Categoria { get; set; }
        public int StatusAtivo { get; set; }

        public static explicit operator ListarProdutoPorIdResponse(Model.Produto request)
        {
            return new ListarProdutoPorIdResponse()
            {
                CodBarras = request.CodBarras,
                NomeProduto = request.NomeProduto,
                DescricaoProduto = request.DescricaoProduto,
                PrecoCusto = request.PrecoCusto,
                PrecoVenda = request.PrecoVenda,
                MargemLucro = request.MargemLucro,
                DataFabricao = request.DataFabricao,
                DataVencimento = request.DataVencimento,
                QuantidadeEstoqueProduto = request.QuantidadeEstoqueProduto,
                Fornecedor = new Model.Fornecedor { Id = request.Id },
                Categoria = new Model.Categoria { Id = request.Id },
                StatusAtivo = request.StatusAtivo
            };
        }
    }
}
