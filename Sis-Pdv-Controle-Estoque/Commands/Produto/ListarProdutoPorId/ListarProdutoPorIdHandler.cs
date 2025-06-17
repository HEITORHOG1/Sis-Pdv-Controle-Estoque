using MediatR;
using prmToolkit.NotificationPattern;

namespace Commands.Produto.ListarProdutoPorId
{
    public class ListarProdutoPorIdHandler : Notifiable, IRequestHandler<ListarProdutoPorIdRequest, ListarProdutoPorIdResponse>
    {
        private readonly IMediator _mediator;
        private readonly IRepositoryProduto _repositoryProduto;

        public ListarProdutoPorIdHandler(IMediator mediator, IRepositoryProduto repositoryProduto)
        {
            _mediator = mediator;
            _repositoryProduto = repositoryProduto;
        }

        public async Task<ListarProdutoPorIdResponse> Handle(ListarProdutoPorIdRequest request, CancellationToken cancellationToken)
        {
            //Valida se o objeto request esta nulo
            if (request == null)
            {
                AddNotification("Request", "");
                return new ListarProdutoPorIdResponse();
            }

            Model.Produto Collection = _repositoryProduto.ObterPor(x => x.Id == request.Id);

            if (Collection == null)
            {
                AddNotification("Request", "");
                return new ListarProdutoPorIdResponse();
            }

            //Cria objeto de resposta
            var response = (ListarProdutoPorIdResponse)Collection;

            ////Retorna o resultado
            return await Task.FromResult(response);
        }
    }
}

