using Sis_Pdv_Controle_Estoque_Form.Dto.Cliente;
using Sis_Pdv_Controle_Estoque_Form.Dto.Colaborador;

namespace Sis_Pdv_Controle_Estoque_Form.Dto.Pedido
{
    public class PedidoDto
    {
        public Guid Id { get; set; }
        public virtual ColaboradorDto? Colaborador { get; set; }
        public virtual ClienteDto? Cliente { get; set; }
        public int Status { get; set; }
        public DateTime DataDoPedido { get; set; }
        public string FormaPagamento { get; set; }
        public decimal TotalPedido { get; set; }
        public Guid ColaboradorId { get; set; }
        public Guid? ClienteId { get; set; }
    }
}
