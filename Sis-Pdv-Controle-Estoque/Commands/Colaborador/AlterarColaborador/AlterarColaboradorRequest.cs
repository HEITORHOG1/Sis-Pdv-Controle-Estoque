using MediatR;

namespace Commands.Colaborador.AlterarColaborador
{
    public class AlterarColaboradorRequest : IRequest<Response>
    {
        public AlterarColaboradorRequest()
        {
            Usuario = new Usuario();
        }
        public Guid Id { get; set; }
        public string NomeColaborador { get; set; }
        public Guid DepartamentoId { get; set; }
        public string CpfColaborador { get; set; }
        public string CargoColaborador { get; set; }
        public string TelefoneColaborador { get; set; }
        public string EmailPessoalColaborador { get; set; }
        public string EmailCorporativo { get; set; }
        public Usuario Usuario { get; set; }
    }
}
