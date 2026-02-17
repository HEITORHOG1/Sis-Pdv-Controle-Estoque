using MediatR;
using prmToolkit.NotificationPattern;

namespace Commands.Fornecedor.AlterarFornecedor
{
    public class AlterarFornecedorHandler : Notifiable, IRequestHandler<AlterarFornecedorRequest, Response>
    {
        private readonly IRepositoryFornecedor _repositoryFornecedor;

        public AlterarFornecedorHandler(IRepositoryFornecedor repositoryFornecedor)
        {
            _repositoryFornecedor = repositoryFornecedor;
        }

        public async Task<Response> Handle(AlterarFornecedorRequest request, CancellationToken cancellationToken)
        {

            Model.Fornecedor Fornecedor = new Model.Fornecedor();

            Fornecedor.AlterarFornecedor(request.Id, request.InscricaoEstadual, request.NomeFantasia, request.Uf, request.Numero,
            request.Complemento, request.Bairro, request.Cidade, request.CepFornecedor, request.StatusAtivo, request.Cnpj, request.Rua);

            var retornoExist = _repositoryFornecedor.Listar().Where(x => x.Id == request.Id);
            if (!retornoExist.Any())
            {
                AddNotification("Fornecedor", "Fornecedor nï¿½o encontrado com o ID informado.");
                return new Response(this);
            }

            Fornecedor = await _repositoryFornecedor.EditarAsync(Fornecedor, cancellationToken);
            //Criar meu objeto de resposta
            var response = new Response(this, Fornecedor);

            return response;
        }
    }
}

