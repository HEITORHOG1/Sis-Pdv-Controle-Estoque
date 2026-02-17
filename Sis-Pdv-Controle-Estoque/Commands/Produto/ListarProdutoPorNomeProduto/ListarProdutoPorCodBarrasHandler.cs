using MediatR;
using prmToolkit.NotificationPattern;

namespace Commands.Produto.ListarProdutoPorNomeProduto
{
    public class ListarProdutoPorCodBarrasHandler : Notifiable, IRequestHandler<ListarProdutoPorCodBarrasRequest, Commands.Response>
    {
        private readonly IRepositoryProduto _repositoryProduto;

        public ListarProdutoPorCodBarrasHandler(IRepositoryProduto repositoryProduto)
        {
            _repositoryProduto = repositoryProduto;
        }

        public Task<Commands.Response> Handle(ListarProdutoPorCodBarrasRequest request, CancellationToken cancellationToken)
        {
            //Valida se o objeto request esta nulo
            if (request == null)
            {
                AddNotification("Erro", "Handle");
                return Task.FromResult(new Commands.Response(this));
            }

            var Collection = _repositoryProduto.Listar().Where(x => x.CodBarras == request.CodBarras);
            if (!Collection.Any())
            {
                AddNotification("ATEN��O", "PRODUTO N�O ENCONTRADA");
                return Task.FromResult(new Commands.Response(this));
            }

            //Criar meu objeto de resposta
            var response = new Commands.Response(this, Collection);
            //Cria objeto de resposta

            ////Retorna o resultado
            return Task.FromResult(response);
        }
    }
}


