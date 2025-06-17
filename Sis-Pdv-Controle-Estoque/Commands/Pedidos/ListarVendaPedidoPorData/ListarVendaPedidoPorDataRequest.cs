using MediatR;

namespace Commands.Pedidos.ListarVendaPedidoPorData
{
    public class ListarVendaPedidoPorDataRequest : IRequest<Sis_Pdv_Controle_Estoque.Commands.Response>
    {
        public ListarVendaPedidoPorDataRequest(DateTime dataInicio, DateTime dataFim)
        {
            DataInicio = dataInicio;
            DataFim = dataFim;
        }

        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
    }
}

