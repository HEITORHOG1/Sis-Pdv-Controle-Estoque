namespace Commands.Produto.ListarProdutoPorId
{
    public class ListarProdutoPorIdResponse
    {
        public Guid? Id { get; set; }
        public string codBarras { get; set; }
        public string nomeProduto { get; set; }
        public string descricaoProduto { get; set; }
        public bool isPerecivel { get; set; }
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
                isPerecivel = request.IsPerecivel,
                Fornecedor = new Model.Fornecedor { Id = request.Id },
                Categoria = new Model.Categoria { Id = request.Id },
                statusAtivo = request.StatusAtivo
            };
        }
    }
}
