using MediatR;

namespace Commands.Cliente.ListarClientePorCpfCnpj
{
    public class ListarClientePorCpfCnpjRequest : IRequest<Commands.Response>
    {
        public ListarClientePorCpfCnpjRequest(string CpfCnpj)
        {
            CpfCnpj = CpfCnpj;
        }

        public string CpfCnpj { get; set; }
    }
}

