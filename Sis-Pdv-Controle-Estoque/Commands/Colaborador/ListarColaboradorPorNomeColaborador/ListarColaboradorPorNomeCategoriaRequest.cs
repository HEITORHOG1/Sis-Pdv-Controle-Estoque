using MediatR;

namespace Commands.Colaborador.ListarColaboradorPorNomeColaborador
{
    public class ListarColaboradorPorNomeColaboradorRequest : IRequest<ListarColaboradorPorNomeColaboradorResponse>
    {
        public ListarColaboradorPorNomeColaboradorRequest(string NomeColaborador)
        {
            this.NomeColaborador = NomeColaborador;
        }
        public string NomeColaborador { get; set; }
    }
}

