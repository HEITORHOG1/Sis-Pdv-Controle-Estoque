using MediatR;
using prmToolkit.NotificationPattern;

namespace Commands.Fornecedor.AdicionarFornecedor
{
    public class AdicionarFornecedorHandler : Notifiable, IRequestHandler<AdicionarFornecedorRequest, Response>
    {
        private readonly IMediator _mediator;
        private readonly IRepositoryFornecedor _repositoryFornecedor;

        public AdicionarFornecedorHandler(IMediator mediator, IRepositoryFornecedor repositoryFornecedor)
        {
            _mediator = mediator;
            _repositoryFornecedor = repositoryFornecedor;
        }

        public async Task<Response> Handle(AdicionarFornecedorRequest request, CancellationToken cancellationToken)
        {
            // Instancia o validador
            var validator = new AdicionarFornecedorRequestValidator();

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

            //Verificar se o usuário já existe
            if (_repositoryFornecedor.Existe(x => x.Cnpj == request.Cnpj))
            {
                AddNotification("Cnpj", "Cnpj ja Cadastrada");
                return new Response(this);
            }

            Model.Fornecedor Fornecedor = new(
                                request.inscricaoEstadual,
                                request.nomeFantasia,
                                request.Uf,
                                request.Numero,
                                request.Complemento,
                                request.Bairro,
                                request.Cidade,
                                request.cepFornecedor,
                                request.statusAtivo,
                                request.Cnpj,
                                request.Rua);

            if (IsInvalid())
            {
                return new Response(this);
            }

            Fornecedor = _repositoryFornecedor.Adicionar(Fornecedor);

            //Criar meu objeto de resposta
            var response = new Response(this, Fornecedor);

            return await Task.FromResult(response);
        }
    }
}
