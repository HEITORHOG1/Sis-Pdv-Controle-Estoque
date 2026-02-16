using MediatR;
using Microsoft.EntityFrameworkCore;
using prmToolkit.NotificationPattern;

namespace Commands.Colaborador.ValidarColaboradorLogin
{
    public class ValidarColaboradorLoginHandler : Notifiable, IRequestHandler<ValidarColaboradorLoginRequest, Commands.Response>
    {
        private readonly IMediator _mediator;
        private readonly IRepositoryColaborador _repositoryColaborador;
        private readonly IRepositoryDepartamento _repositoryDepartamento;
        public ValidarColaboradorLoginHandler(IMediator mediator, IRepositoryColaborador repositoryColaborador,
            IRepositoryDepartamento repositoryDepartamento)
        {
            _mediator = mediator;
            _repositoryColaborador = repositoryColaborador;
            _repositoryDepartamento = repositoryDepartamento;
        }

        public async Task<Response> Handle(ValidarColaboradorLoginRequest request, CancellationToken cancellationToken)
        {
            // Instancia o validador
            var validator = new ValidarColaboradorLoginRequestValidator();

            // Valida a requisição
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            // Se não passou na validação, adiciona as falhas como notificações
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    AddNotification(error.PropertyName, error.ErrorMessage);
                }

                return new Response(this);
            }

            var result = _repositoryColaborador.Listar()
                                    .Include(x => x.Usuario)
                                    .Where(x => x.Usuario.Login == request.Login && x.Usuario.Senha == request.Senha).FirstOrDefault();
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
            return await Task.FromResult(response);
        }
    }
}

