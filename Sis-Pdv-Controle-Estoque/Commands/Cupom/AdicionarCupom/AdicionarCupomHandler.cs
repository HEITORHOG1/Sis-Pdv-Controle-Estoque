using Interfaces;
using MediatR;
using prmToolkit.NotificationPattern;

namespace Commands.Cupom.AdicionarCupom
{
    public class AdicionarCupomHandler : Notifiable, IRequestHandler<AdicionarCupomRequest, Response>
    {
        private readonly IRepositoryCupom _repositoryCupom;
        private readonly IRepositoryPedido _repositoryPedido;

        public AdicionarCupomHandler(IRepositoryCupom repositoryCupom, IRepositoryPedido repositoryPedido)
        {
            _repositoryCupom = repositoryCupom;
            _repositoryPedido = repositoryPedido;
        }

        public async Task<Response> Handle(AdicionarCupomRequest request, CancellationToken cancellationToken)
        {
            // Verificar se o pedido existe
            var pedido = await _repositoryPedido.ObterPorIdAsync(request.PedidoId, cancellationToken);
            if (pedido == null)
            {
                AddNotification("PedidoId", "Pedido não encontrado.");
                return new Response(this);
            }

            // Verificar se já existe cupom para este pedido
            if (await _repositoryCupom.ExisteAsync(x => x.PedidoId == request.PedidoId, cancellationToken))
            {
                AddNotification("PedidoId", "Já existe um cupom emitido para este pedido.");
                return new Response(this);
            }

            var cupom = new Model.Cupom(
                request.PedidoId,
                request.DataEmissao,
                request.NumeroSerie,
                request.ChaveAcesso,
                request.ValorTotal,
                request.DocumentoCliente);

            if (IsInvalid())
                return new Response(this);

            cupom = await _repositoryCupom.AdicionarAsync(cupom, cancellationToken);

            return new Response(this, cupom);
        }
    }
}
