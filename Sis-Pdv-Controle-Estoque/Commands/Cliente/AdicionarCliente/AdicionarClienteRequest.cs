using MediatR;

namespace Commands.Cliente.AdicionarCliente
{
    public class AdicionarClienteRequest : IRequest<Response>
    {
        public string CpfCnpj { get; set; }
        public string TipoCliente { get; set; }
    }
}
