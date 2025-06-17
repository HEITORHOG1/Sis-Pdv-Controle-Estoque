using MediatR;
using prmToolkit.NotificationPattern;

namespace Commands.Departamento.AdicionarDepartamento
{
    public class AdicionarDepartamentoHandler : Notifiable, IRequestHandler<AdicionarDepartamentoRequest, Response>
    {
        private readonly IMediator _mediator;
        private readonly IRepositoryDepartamento _repositoryDepartamento;

        public AdicionarDepartamentoHandler(IMediator mediator, IRepositoryDepartamento repositoryDepartamento)
        {
            _mediator = mediator;
            _repositoryDepartamento = repositoryDepartamento;
        }

        public async Task<Response> Handle(AdicionarDepartamentoRequest request, CancellationToken cancellationToken)
        {
            //Validar se o requeste veio preenchido
            if (request == null)
            {
                AddNotification("NomeDepartamento", "Departamento não pode estar em branco... Preencha uma Departamento");
                return new Response(this);
            }
            if (request.NomeDepartamento == "")
            {
                AddNotification("NomeDepartamento", "Departamento não pode estar em branco... Preencha uma Departamento");
                return new Response(this);
            }

            //Verificar se o usuário já existe
            if (_repositoryDepartamento.Existe(x => x.NomeDepartamento == request.NomeDepartamento))
            {
                AddNotification("NomeDepartamento", "Departamento ja Cadastrada");
                return new Response(this);
            }

            Model.Departamento Departamento = new(request.NomeDepartamento);

            if (IsInvalid())
            {
                return new Response(this);
            }

            Departamento = _repositoryDepartamento.Adicionar(Departamento);

            //Criar meu objeto de resposta
            var result = new { Departamento.Id, Departamento.NomeDepartamento };

            //Criar meu objeto de resposta
            var response = new Response(this, Departamento);

            return await Task.FromResult(response);
        }
    }
}
