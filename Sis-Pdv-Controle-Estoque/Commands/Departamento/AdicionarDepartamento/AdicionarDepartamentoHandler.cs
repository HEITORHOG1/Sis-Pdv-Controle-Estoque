using MediatR;
using prmToolkit.NotificationPattern;

namespace Commands.Departamento.AdicionarDepartamento
{
    public class AdicionarDepartamentoHandler : Notifiable, IRequestHandler<AdicionarDepartamentoRequest, Response>
    {
        private readonly IRepositoryDepartamento _repositoryDepartamento;

        public AdicionarDepartamentoHandler(IRepositoryDepartamento repositoryDepartamento)
        {
            _repositoryDepartamento = repositoryDepartamento;
        }

        public Task<Response> Handle(AdicionarDepartamentoRequest request, CancellationToken cancellationToken)
        {
            //Validar se o requeste veio preenchido
            if (request == null)
            {
                AddNotification("NomeDepartamento", "Departamento não pode estar em branco... Preencha uma Departamento");
                return Task.FromResult(new Response(this));
            }
            if (request.NomeDepartamento == "")
            {
                AddNotification("NomeDepartamento", "Departamento não pode estar em branco... Preencha uma Departamento");
                return Task.FromResult(new Response(this));
            }

            //Verificar se o usuário já existe
            if (_repositoryDepartamento.Existe(x => x.NomeDepartamento == request.NomeDepartamento))
            {
                AddNotification("NomeDepartamento", "Departamento ja Cadastrada");
                return Task.FromResult(new Response(this));
            }

            Model.Departamento Departamento = new(request.NomeDepartamento);

            if (IsInvalid())
            {
                return Task.FromResult(new Response(this));
            }

            Departamento = _repositoryDepartamento.Adicionar(Departamento);

            //Criar meu objeto de resposta
            var result = new { Departamento.Id, Departamento.NomeDepartamento };

            //Criar meu objeto de resposta
            var response = new Response(this, Departamento);

            return Task.FromResult(response);
        }
    }
}
