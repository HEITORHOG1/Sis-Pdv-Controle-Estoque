using MediatR;
using prmToolkit.NotificationPattern;

namespace Commands.Departamento.AlterarDepartamento
{
    public class AlterarDepartamentoHandler : Notifiable, IRequestHandler<AlterarDepartamentoRequest, Sis_Pdv_Controle_Estoque.Commands.Response>
    {
        private readonly IMediator _mediator;
        private readonly IRepositoryDepartamento _repositoryDepartamento;

        public AlterarDepartamentoHandler(IMediator mediator, IRepositoryDepartamento repositoryDepartamento)
        {
            _mediator = mediator;
            _repositoryDepartamento = repositoryDepartamento;
        }

        public async Task<Sis_Pdv_Controle_Estoque.Commands.Response> Handle(AlterarDepartamentoRequest request, CancellationToken cancellationToken)
        {
            //Validar se o requeste veio preenchido
            if (request == null)
            {
                AddNotification("Departamento", "");
                return new Sis_Pdv_Controle_Estoque.Commands.Response(this);
            }

            Sis_Pdv_Controle_Estoque.Model.Departamento departamento = new Sis_Pdv_Controle_Estoque.Model.Departamento();

            departamento.AlterarDepartamento(request.Id, request.NomeDepartamento);
            var retornoExist = _repositoryDepartamento.Listar().Where(x => x.Id == request.Id);
            if (!retornoExist.Any())
            {
                AddNotification("Departamento", "");
                return new Sis_Pdv_Controle_Estoque.Commands.Response(this);
            }

            departamento = _repositoryDepartamento.Editar(departamento);

            var result = new { Id = departamento.Id, NomeDepartamento = departamento.NomeDepartamento };

            //Criar meu objeto de resposta
            var response = new Sis_Pdv_Controle_Estoque.Commands.Response(this, result);

            return await Task.FromResult(response);
        }
    }
}
