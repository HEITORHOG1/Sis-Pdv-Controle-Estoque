using MediatR;

namespace Commands.Pedidos.ListarPedidoPorNomeCpfCnpj
{
    public class ListarPedidoPorNomeCpfCnpjRequest : IRequest<Commands.Response>
    {
        public ListarPedidoPorNomeCpfCnpjRequest(string cnpj)
        {
            Cnpj = cnpj;
        }

        public string Cnpj { get; set; }
    }
}

