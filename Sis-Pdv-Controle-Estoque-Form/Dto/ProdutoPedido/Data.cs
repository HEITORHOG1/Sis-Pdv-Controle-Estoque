using Sis_Pdv_Controle_Estoque_Form.Dto.Pedido;

namespace Sis_Pdv_Controle_Estoque_Form.Dto.ProdutoPedido
{
    public  class Data
    {
        public Guid Id { get; set; }
        public PedidoDto? Pedido { get; set; }
        public ProdutoPedidoDto? Produto { get; set; }
        public string? codBarras { get; set; }
        public int? quantidadeItemPedido { get; set; }
        public decimal? totalProdutoPedido { get; set; }
    }
}
