using MediatR;

namespace Commands.Colaborador.ValidarColaboradorLogin
{
    public class ValidarColaboradorLoginRequest : IRequest<Response>
    {
        public ValidarColaboradorLoginRequest(string login, string senha)
        {
            Senha = senha;
            Login = login;
        }
        public string Login { get; set; }
        public string Senha { get; set; }
    }
}
