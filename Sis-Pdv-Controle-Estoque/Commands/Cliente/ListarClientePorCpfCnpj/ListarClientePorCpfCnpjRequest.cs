using MediatR;

namespace Commands.Cliente.ListarClientePorCpfCnpj
{
    public class ListarClientePorCpfCnpjRequest : IRequest<Commands.Response>
    {
        public ListarClientePorCpfCnpjRequest(string cpfCnpj)
        {
            CpfCnpj = cpfCnpj;
        }

        public string CpfCnpj { get; set; }
    }
}

