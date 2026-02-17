using Interfaces;
using MediatR;
using prmToolkit.NotificationPattern;

namespace Commands.Colaborador.ValidarColaboradorLogin
{
    public class ValidarColaboradorLoginHandler : Notifiable, IRequestHandler<ValidarColaboradorLoginRequest, Commands.Response>
    {
        private readonly IRepositoryColaborador _repositoryColaborador;
        private readonly IRepositoryDepartamento _repositoryDepartamento;
        public ValidarColaboradorLoginHandler(IRepositoryColaborador repositoryColaborador,
            IRepositoryDepartamento repositoryDepartamento)
        {
            _repositoryColaborador = repositoryColaborador;
            _repositoryDepartamento = repositoryDepartamento;
        }

        public async Task<Response> Handle(ValidarColaboradorLoginRequest request, CancellationToken cancellationToken)
        {
            var result = await _repositoryColaborador.ObterPorAsync(
                x => x.Usuario.Login == request.Login && x.Usuario.Senha == request.Senha,
                cancellationToken,
                x => x.Usuario);

            if (result == null)
            {
                AddNotification("Atenção", "Login não encontrado");
                return new Response(this);
            }
            if (result.Usuario.StatusAtivo == false)
            {
                AddNotification("Atenção", "Usuario Inativo no Sistema");
                return new Response(this);
            }

            //Cria objeto de resposta
            var response = new Commands.Response(this, result);

            ////Retorna o resultado
            return response;
        }
    }
}
