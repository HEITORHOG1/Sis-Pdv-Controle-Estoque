using MediatR;
using prmToolkit.NotificationPattern;

namespace Commands.Produto.AtualizarEstoque
{
    public class AtualizarEstoqueHandler : Notifiable, IRequestHandler<AtualizarEstoqueRequest, Response>
    {
        private readonly IRepositoryProduto _repositoryProduto;

        public AtualizarEstoqueHandler(IRepositoryProduto repositoryProduto)
        {
            _repositoryProduto = repositoryProduto;
        }

        public async Task<Response> Handle(AtualizarEstoqueRequest request, CancellationToken cancellationToken)
        {

            Model.Produto Produto = new Model.Produto();

            var retornoExist = _repositoryProduto.Listar().Where(x => x.Id == request.Id).FirstOrDefault();

            if (retornoExist == null)
            {
                AddNotification("Produto", "Produto não encontrado com o ID informado.");
                return new Response(this);
            }
            if (retornoExist.QuantidadeEstoqueProduto <= 0)
            {
                AddNotification("Produto", "Produto Sem Estoque");
                return new Response(this);
            }
            if (request.QuantidadeEstoqueProduto > retornoExist.QuantidadeEstoqueProduto)
            {
                AddNotification("Produto", "Quantidade em estoque " + retornoExist.QuantidadeEstoqueProduto);
                return new Response(this);
            }

            retornoExist.QuantidadeEstoqueProduto = retornoExist.QuantidadeEstoqueProduto - request.QuantidadeEstoqueProduto;
            Produto = await _repositoryProduto.EditarAsync(retornoExist, cancellationToken);

            //Criar meu objeto de resposta
            var response = new Response(this, Produto);
            

            Console.WriteLine($"Produto atualizado. Novo estoque: {Produto.QuantidadeEstoqueProduto}");

            return response;
        }
    }
}

