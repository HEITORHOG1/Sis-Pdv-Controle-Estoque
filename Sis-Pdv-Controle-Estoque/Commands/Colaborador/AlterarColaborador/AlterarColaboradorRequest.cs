using MediatR;
using Model;

namespace Commands.Colaborador.AlterarColaborador
{
    public class AlterarColaboradorRequest : IRequest<Response>
    {
        public AlterarColaboradorRequest()
        {
            Usuario = new Usuario();
        }
        public Guid Id { get; set; }
        public string nomeColaborador { get; set; }
        public Guid DepartamentoId { get; set; }
        public string cpfColaborador { get; set; }
        public string cargoColaborador { get; set; }
        public string telefoneColaborador { get; set; }
        public string emailPessoalColaborador { get; set; }
        public string emailCorporativo { get; set; }
        public Usuario Usuario { get; set; }
    }
}
