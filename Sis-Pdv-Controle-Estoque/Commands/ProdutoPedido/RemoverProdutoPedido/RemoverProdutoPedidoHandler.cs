using MediatR;
using prmToolkit.NotificationPattern;

namespace Commands.ProdutoPedido.RemoverProdutoPedido

{
    public class RemoverProdutoPedidoHandler : Notifiable, IRequestHandler<RemoverProdutoPedidoResquest, Sis_Pdv_Controle_Estoque.Commands.Response>
    {
        private readonly IMediator _mediator;
        private readonly IRepositoryProdutoPedido _repositoryProdutoPedido;

        public RemoverProdutoPedidoHandler(IMediator mediator, IRepositoryProdutoPedido repositoryProdutoPedido)
        {
            _mediator = mediator;
            _repositoryProdutoPedido = repositoryProdutoPedido;
        }

        public async Task<Sis_Pdv_Controle_Estoque.Commands.Response> Handle(RemoverProdutoPedidoResquest request, CancellationToken cancellationToken)
        {
            //Valida se o objeto request esta nulo
            if (request == null)
            {
                AddNotification("Request", "");
                return new Sis_Pdv_Controle_Estoque.Commands.Response(this);
            }

            Sis_Pdv_Controle_Estoque.Model.ProdutoPedido ProdutoPedido = _repositoryProdutoPedido.ObterPorId(request.Id);

            if (ProdutoPedido == null)
            {
                AddNotification("Request", "");
                return new Sis_Pdv_Controle_Estoque.Commands.Response(this);
            }

            _repositoryProdutoPedido.Remover(ProdutoPedido);

            var result = new { Id = ProdutoPedido.Id };

            //Cria objeto de resposta
            var response = new Sis_Pdv_Controle_Estoque.Commands.Response(this, result);

            ////Retorna o resultado
            return await Task.FromResult(response);
        }
    }
}
