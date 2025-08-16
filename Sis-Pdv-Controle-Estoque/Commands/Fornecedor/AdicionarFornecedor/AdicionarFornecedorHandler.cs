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
            // Validation is now handled by the ValidationBehavior pipeline

            //Verificar se o fornecedor já existe
            if (_repositoryFornecedor.Existe(x => x.Cnpj == request.Cnpj))
            {
                AddNotification("Cnpj", "Fornecedor já cadastrado com este CNPJ");
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
