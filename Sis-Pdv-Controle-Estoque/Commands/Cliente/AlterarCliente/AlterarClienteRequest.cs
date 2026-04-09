using MediatR;

namespace Commands.Cliente.AlterarCliente
{
    public class AlterarClienteRequest : IRequest<Response>
    {
        public Guid Id { get; set; }
        public string CpfCnpj { get; set; }
        public string TipoCliente { get; set; }
    }
}
