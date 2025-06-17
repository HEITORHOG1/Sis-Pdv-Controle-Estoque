using MediatR;
using prmToolkit.NotificationPattern;

namespace Commands.Fornecedor.AlterarFornecedor
{
    public class AlterarFornecedorHandler : Notifiable, IRequestHandler<AlterarFornecedorRequest, Response>
    {
        private readonly IMediator _mediator;
        private readonly IRepositoryFornecedor _repositoryFornecedor;

        public AlterarFornecedorHandler(IMediator mediator, IRepositoryFornecedor repositoryFornecedor)
        {
            _mediator = mediator;
            _repositoryFornecedor = repositoryFornecedor;
        }

        public async Task<Response> Handle(AlterarFornecedorRequest request, CancellationToken cancellationToken)
        {
            // Instancia o validador
            var validator = new AlterarFornecedorRequestValidator();

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

            Model.Fornecedor Fornecedor = new Model.Fornecedor();

            Fornecedor.AlterarFornecedor(request.Id, request.inscricaoEstadual, request.nomeFantasia, request.Uf, request.Numero,
            request.Complemento, request.Bairro, request.Cidade, request.cepFornecedor, request.statusAtivo, request.Cnpj, request.Rua);

            var retornoExist = _repositoryFornecedor.Listar().Where(x => x.Id == request.Id);
            if (!retornoExist.Any())
            {
                AddNotification("Fornecedor", "");
                return new Response(this);
            }

            Fornecedor = _repositoryFornecedor.Editar(Fornecedor);
            //Criar meu objeto de resposta
            var response = new Response(this, Fornecedor);

            return await Task.FromResult(response);
        }
    }
}
