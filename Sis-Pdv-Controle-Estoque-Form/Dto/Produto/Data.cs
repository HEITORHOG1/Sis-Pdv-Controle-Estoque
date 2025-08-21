namespace Sis_Pdv_Controle_Estoque_Form.Dto.Produto
{
    public class Data
    {
        public Guid Id { get; set; }
        public string codBarras { get; set; }
        public string nomeProduto { get; set; }
        public string descricaoProduto { get; set; }
        public bool isPerecivel { get; set; }
        public Guid FornecedorId { get; set; }
        public Guid CategoriaId { get; set; }
        public string NomeFornecedor { get; set; }
        public string NomeCategoria { get; set; }
        public int statusAtivo { get; set; }
        
        // Campos legados mantidos para compatibilidade temporária com APIs antigas
        public decimal precoCusto { get; set; }
        public decimal precoVenda { get; set; }
        public decimal margemLucro { get; set; }
        public DateTime dataFabricao { get; set; }
        public DateTime dataVencimento { get; set; }
        public int quatidadeEstoqueProduto { get; set; }
    }
}
