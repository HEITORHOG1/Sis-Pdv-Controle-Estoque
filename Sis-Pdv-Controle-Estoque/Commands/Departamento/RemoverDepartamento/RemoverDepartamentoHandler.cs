using MediatR;
using prmToolkit.NotificationPattern;

namespace Commands.Departamento.RemoverDepartamento

{
    public class RemoverDepartamentoHandler : Notifiable, IRequestHandler<RemoverDepartamentoRequest, Commands.Response>
    {
        private readonly IRepositoryDepartamento _repositoryDepartamento;

        public RemoverDepartamentoHandler(IRepositoryDepartamento repositoryDepartamento)
        {
            _repositoryDepartamento = repositoryDepartamento;
        }

        public Task<Commands.Response> Handle(RemoverDepartamentoRequest request, CancellationToken cancellationToken)
        {
            //Valida se o objeto request esta nulo
            if (request == null)
            {
                AddNotification("Request", "A requisição não pode ser nula.");
                return Task.FromResult(new Commands.Response(this));
            }

            Model.Departamento Departamento = _repositoryDepartamento.ObterPorId(request.Id);

            if (Departamento == null)
            {
                AddNotification("Request", "A requisição não pode ser nula.");
                return Task.FromResult(new Commands.Response(this));
            }

            _repositoryDepartamento.Remover(Departamento);

            var result = new { Id = Departamento.Id };

            //Cria objeto de resposta
            var response = new Commands.Response(this, result);

            ////Retorna o resultado
            return Task.FromResult(response);
        }
    }
}
