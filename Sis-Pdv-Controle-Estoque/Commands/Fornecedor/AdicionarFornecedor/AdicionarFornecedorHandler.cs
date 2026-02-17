using MediatR;
using prmToolkit.NotificationPattern;

namespace Commands.Fornecedor.AdicionarFornecedor
{
    public class AdicionarFornecedorHandler : Notifiable, IRequestHandler<AdicionarFornecedorRequest, Response>
    {
        private readonly IRepositoryFornecedor _repositoryFornecedor;

        public AdicionarFornecedorHandler(IRepositoryFornecedor repositoryFornecedor)
        {
            _repositoryFornecedor = repositoryFornecedor;
        }

        public async Task<Response> Handle(AdicionarFornecedorRequest request, CancellationToken cancellationToken)
        {
            // Validation is now handled by the ValidationBehavior pipeline

            //Verificar se o fornecedor j� existe
            if (await _repositoryFornecedor.ExisteAsync(x => x.Cnpj == request.Cnpj))
            {
                AddNotification("Cnpj", "Fornecedor j� cadastrado com este CNPJ");
                return new Response(this);
            }

            Model.Fornecedor Fornecedor = new(
                                request.InscricaoEstadual,
                                request.NomeFantasia,
                                request.Uf,
                                request.Numero,
                                request.Complemento,
                                request.Bairro,
                                request.Cidade,
                                request.CepFornecedor,
                                request.StatusAtivo,
                                request.Cnpj,
                                request.Rua);

            if (IsInvalid())
            {
                return new Response(this);
            }

            Fornecedor = await _repositoryFornecedor.AdicionarAsync(Fornecedor, cancellationToken);

            //Criar meu objeto de resposta
            var response = new Response(this, Fornecedor);

            return response;
        }
    }
}
