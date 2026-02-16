namespace Commands.Produto.ListarProdutoPorId
{
    public class ListarProdutoPorIdResponse
    {
        public Guid? Id { get; set; }
        public string codBarras { get; set; }
        public string nomeProduto { get; set; }
        public string descricaoProduto { get; set; }
        public decimal precoCusto { get; set; }
        public decimal precoVenda { get; set; }
        public decimal margemLucro { get; set; }
        public DateTime dataFabricao { get; set; }
        public DateTime dataVencimento { get; set; }
        public int quatidadeEstoqueProduto { get; set; }
        public Model.Fornecedor Fornecedor { get; set; }
        public Model.Categoria Categoria { get; set; }
        public int statusAtivo { get; set; }

        public static explicit operator ListarProdutoPorIdResponse(Model.Produto request)
        {
            return new ListarProdutoPorIdResponse()
            {
                codBarras = request.CodBarras,
                nomeProduto = request.NomeProduto,
                descricaoProduto = request.DescricaoProduto,
                precoCusto = request.PrecoCusto,
                precoVenda = request.PrecoVenda,
                margemLucro = request.MargemLucro,
                dataFabricao = request.DataFabricao,
                dataVencimento = request.DataVencimento,
                quatidadeEstoqueProduto = request.QuatidadeEstoqueProduto,
                Fornecedor = new Model.Fornecedor { Id = request.Id },
                Categoria = new Model.Categoria { Id = request.Id },
                statusAtivo = request.StatusAtivo
            };
        }
    }
}
