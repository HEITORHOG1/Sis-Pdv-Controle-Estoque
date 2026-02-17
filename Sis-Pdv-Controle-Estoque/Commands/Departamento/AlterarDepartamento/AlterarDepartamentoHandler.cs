using MediatR;
using prmToolkit.NotificationPattern;

namespace Commands.Departamento.AlterarDepartamento
{
    public class AlterarDepartamentoHandler : Notifiable, IRequestHandler<AlterarDepartamentoRequest, Commands.Response>
    {
        private readonly IRepositoryDepartamento _repositoryDepartamento;

        public AlterarDepartamentoHandler(IRepositoryDepartamento repositoryDepartamento)
        {
            _repositoryDepartamento = repositoryDepartamento;
        }

        public Task<Commands.Response> Handle(AlterarDepartamentoRequest request, CancellationToken cancellationToken)
        {
            //Validar se o requeste veio preenchido
            if (request == null)
            {
                AddNotification("Departamento", "Departamento não encontrado com o ID informado.");
                return Task.FromResult(new Commands.Response(this));
            }

            Model.Departamento departamento = new Model.Departamento();

            departamento.AlterarDepartamento(request.Id, request.NomeDepartamento);
            var retornoExist = _repositoryDepartamento.Listar().Where(x => x.Id == request.Id);
            if (!retornoExist.Any())
            {
                AddNotification("Departamento", "Departamento não encontrado com o ID informado.");
                return Task.FromResult(new Commands.Response(this));
            }

            departamento = _repositoryDepartamento.Editar(departamento);

            var result = new { Id = departamento.Id, NomeDepartamento = departamento.NomeDepartamento };

            //Criar meu objeto de resposta
            var response = new Commands.Response(this, result);

            return Task.FromResult(response);
        }
    }
}
