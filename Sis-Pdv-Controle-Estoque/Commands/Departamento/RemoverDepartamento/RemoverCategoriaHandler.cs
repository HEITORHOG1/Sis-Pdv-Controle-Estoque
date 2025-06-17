using MediatR;
using prmToolkit.NotificationPattern;

namespace Commands.Departamento.RemoverDepartamento

{
    public class RemoverDepartamentoHandler : Notifiable, IRequestHandler<RemoverDepartamentoResquest, Commands.Response>
    {
        private readonly IMediator _mediator;
        private readonly IRepositoryDepartamento _repositoryDepartamento;

        public RemoverDepartamentoHandler(IMediator mediator, IRepositoryDepartamento repositoryDepartamento)
        {
            _mediator = mediator;
            _repositoryDepartamento = repositoryDepartamento;
        }

        public async Task<Commands.Response> Handle(RemoverDepartamentoResquest request, CancellationToken cancellationToken)
        {
            //Valida se o objeto request esta nulo
            if (request == null)
            {
                AddNotification("Request", "");
                return new Commands.Response(this);
            }

            Model.Departamento Departamento = _repositoryDepartamento.ObterPorId(request.Id);

            if (Departamento == null)
            {
                AddNotification("Request", "");
                return new Commands.Response(this);
            }

            _repositoryDepartamento.Remover(Departamento);

            var result = new { Id = Departamento.Id };

            //Cria objeto de resposta
            var response = new Commands.Response(this, result);

            ////Retorna o resultado
            return await Task.FromResult(response);
        }
    }
}
