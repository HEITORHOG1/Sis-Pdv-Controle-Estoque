using Sis_Pdv_Controle_Estoque_Form.Dto.Cliente;
using Sis_Pdv_Controle_Estoque_Form.Dto.Colaborador;

namespace Sis_Pdv_Controle_Estoque_Form.Dto.Pedido
{
    public  class DataGrid
    {
        public DateTime dataDoPedido { get; set; }
        public string formaPagamento { get; set; }
        public decimal totalPedido { get; set; }
        public Guid Id { get; set; }
    }
}
